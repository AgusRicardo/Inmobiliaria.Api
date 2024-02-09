using Inmobiliaria.Models;
using Inmobiliaria.Request;
using Inmobiliaria.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria.Controllers
{
    public class InquilinosController : ApiController
    {
        private readonly ITokenService _tokenService;
        public InquilinosController(QczbbchrContext context, ITokenService tokenService) : base(context)
        {
            _tokenService = tokenService;
        }

        [HttpGet("GetInquilinos")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult> GetInquilinos()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var jsonToken = _tokenService.DecodeToken(token);

                var claimValue = jsonToken.Claims.ElementAt(3).Value;

                var propietarios = await _context.Propietarios
                    .Where(p => p.inmobiliaria_id == int.Parse(claimValue))
                    .OrderByDescending(p => p.id_propietario)
                    .ToListAsync();

                if (propietarios == null || propietarios.Count == 0)
                {
                    return NotFound(new { statusCode = StatusCodes.Status200OK, message = "No hay propietarios disponibles." });
                }

                var propietariosIds = propietarios.Select(p => p.id_propietario).ToList();

                var contratos = await _context.Contratos
                    .Include(c => c.Propietario)
                    .Where(c => propietariosIds.Contains(c.id_propietario))
                    .Include(c => c.Propiedad)
                    .Include(c => c.Inquilino)
                    .Include(c => c.Garante)
                    .Include(c => c.Estado)
                    .Select(c => new
                    {
                        c.id_contrato,
                        Inquilino = new
                        {
                            c.Inquilino.id_inquilino,
                            c.Inquilino.nombre,
                            c.Inquilino.apellido
                        }
                    })
                    .OrderByDescending(p => p.id_contrato).ToListAsync();

                var inquilinoInContract = contratos.Select(c => c.Inquilino.id_inquilino).ToList();

                var inquilinos = await _context.Inquilinos
                    .Where(p => inquilinoInContract.Contains(p.id_inquilino))
                    .OrderByDescending(p => p.id_inquilino)
                    .ToListAsync();

                if (inquilinos == null || inquilinos.Count == 0)
                {
                    return NotFound(new { statusCode = StatusCodes.Status200OK, message = "No hay inquilinos disponibles."});
                }
                return Ok(inquilinos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpGet("GetInquilinoById/{id_inquilino}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult> GetInquilinoById(int id_inquilino)
        {
            try
            {
                var inquilino = await _context.Inquilinos.FindAsync(id_inquilino);

                if (inquilino == null)
                {
                    var errorMessage = new { message = $"No se encontró ningún inquilino con ID {id_inquilino}" };
                    return NotFound(errorMessage);
                }

                return Ok(inquilino);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpPost("CrearInquilino")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        public async Task<ActionResult> CrearInquilino([FromBody] CrearInquilinoRequest nuevoInquilinoRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var nuevoInquilino = new Inquilino
                {
                    nombre = nuevoInquilinoRequest.nombre,
                    apellido = nuevoInquilinoRequest.apellido,
                    dni = nuevoInquilinoRequest.dni,
                    fecha_alta = DateOnly.FromDateTime(DateTime.Now)
                };

                _context.Inquilinos.Add(nuevoInquilino);
                await _context.SaveChangesAsync();

                var successMessage = $"Se ha creado un nuevo inquilino exitosamente";

                return Ok(new { Message = successMessage, InquilinoId = nuevoInquilino.id_inquilino });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpPut("EditarInquilino/{id_inquilino}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult> EditarPersona(int id_inquilino, [FromBody] EditarInquilinoRequest editarInquilinoRequest)
        {
            try
            {
                var inquilino = await _context.Inquilinos.FindAsync(id_inquilino);

                if (inquilino == null)
                {
                    var errorMessage = new { statusCode = 404, message = $"No se encontró ningún inquilino con ID {id_inquilino}" };
                    return NotFound(errorMessage);
                }

                if (!string.IsNullOrEmpty(editarInquilinoRequest.nombre))
                {
                    inquilino.nombre = editarInquilinoRequest.nombre;
                }

                if (!string.IsNullOrEmpty(editarInquilinoRequest.apellido))
                {
                    inquilino.apellido = editarInquilinoRequest.apellido;
                }

                if (!string.IsNullOrEmpty(editarInquilinoRequest.dni))
                {
                    inquilino.dni = editarInquilinoRequest.dni;
                }

                await _context.SaveChangesAsync();

                var successMessage = $"Se ha editado un inquilino con ID {id_inquilino} exitosamente";
                return Ok(new { statusCode = 200, InquilinoId = inquilino.id_inquilino, Message = successMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
