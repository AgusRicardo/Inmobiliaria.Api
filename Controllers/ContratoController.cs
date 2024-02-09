using Inmobiliaria.Models;
using Inmobiliaria.Request;
using Inmobiliaria.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Inmobiliaria.Controllers
{
    public class ContratoController : ApiController
    {
        private readonly ITokenService _tokenService;
        public ContratoController(QczbbchrContext context, ITokenService tokenService) : base(context)
        {
            _tokenService = tokenService;
        }

        [HttpGet("GetContratos")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult> GetContratos()
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
                        Propietario = new
                        {
                            c.Propietario.id_propietario,
                            c.Propietario.nombre,
                            c.Propietario.apellido
                        },
                        Propiedad = new
                        {
                            c.Propiedad.id_propiedad,
                            c.Propiedad.tipo,
                            c.Propiedad.direccion
                        },
                        Inquilino = new
                        {
                            c.Inquilino.id_inquilino,
                            c.Inquilino.nombre,
                            c.Inquilino.apellido
                        },
                        Garante = new
                        {
                            c.Garante.id_garante,
                            c.Garante.nombre,
                            c.Garante.apellido
                        },
                        c.fecha_inicio,
                        c.fecha_fin,
                        c.monto,
                        Estado = new
                        {
                            c.Estado.id_estado,
                            c.Estado.descripcion
                        },
                        c.fecha_alta
                    })
                    .OrderByDescending(p => p.id_contrato).ToListAsync();

                if (contratos == null || contratos.Count == 0)
                {
                    return NotFound(new { statusCode = StatusCodes.Status200OK, message = "No hay contratos disponibles." });
                }

                return Ok(contratos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpGet("GetContratoById/{id_contrato}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult> GetContratoById(int id_contrato)
        {
            try
            {
                var contrato = await _context.Contratos.FindAsync(id_contrato);

                if (contrato == null)
                {
                    var errorMessage = new { message = $"No se encontró ningún contrato con ID {id_contrato}" };
                    return NotFound(errorMessage);
                }

                return Ok(contrato);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpPost("CrearContrato")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        public async Task<ActionResult> CrearContrato([FromBody] CrearCrontratoRequest crearCrontratoRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var nuevoContrato = new Contrato
                {
                    id_inquilino = crearCrontratoRequest.id_inquilino,
                    id_propiedad = crearCrontratoRequest.id_propiedad,
                    fecha_inicio = crearCrontratoRequest.fecha_inicio,
                    fecha_fin = crearCrontratoRequest.fecha_fin,
                    monto = crearCrontratoRequest.monto,
                    id_garante = crearCrontratoRequest.id_garante,
                    fecha_alta = DateOnly.FromDateTime(DateTime.Now)
                };

                _context.Contratos.Add(nuevoContrato);
                await _context.SaveChangesAsync();

                var successMessage = $"Se ha creado un nuevo contrato exitosamente";

                return Ok(new { Message = successMessage, GaranteId = nuevoContrato.id_garante });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpPut("EditarContrato/{id_contrato}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult> EditarContrato(int id_contrato, [FromBody] EditarContratoRequest editarContratoRequest)
        {
            try
            {
                var contrato = await _context.Contratos.FindAsync(id_contrato);

                if (contrato == null)
                {
                    var errorMessage = new { statusCode = 404, message = $"No se encontró ningún contrato con ID {id_contrato}" };
                    return NotFound(errorMessage);
                }

                if (editarContratoRequest.id_propietario != null)
                {
                    contrato.id_propietario = editarContratoRequest.id_propietario;
                }

                if (editarContratoRequest.id_propiedad != null)
                {
                    contrato.id_propiedad = editarContratoRequest.id_propiedad;
                }

                if (editarContratoRequest.id_inquilino != null)
                {
                    contrato.id_inquilino = editarContratoRequest.id_inquilino;
                }

                if (editarContratoRequest.id_garante != null)
                {
                    contrato.id_garante = editarContratoRequest.id_garante;
                }

                if (editarContratoRequest.fecha_inicio != default(DateOnly))
                {
                    contrato.fecha_inicio = editarContratoRequest.fecha_inicio;
                }

                if (editarContratoRequest.fecha_fin != default(DateOnly))
                {
                    contrato.fecha_fin = editarContratoRequest.fecha_fin;
                }

                if (editarContratoRequest.monto != null)
                {
                    contrato.monto = editarContratoRequest.monto;
                }

                await _context.SaveChangesAsync();

                var successMessage = $"Se ha editado un contrato con ID {id_contrato} exitosamente";
                return Ok(new { statusCode = 200, ContratoId = contrato.id_contrato, Message = successMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
