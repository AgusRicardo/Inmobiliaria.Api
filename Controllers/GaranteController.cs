using Inmobiliaria.Models;
using Inmobiliaria.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria.Controllers
{
    public class GaranteController : ApiController
    {
        public GaranteController(QczbbchrContext context) : base(context)
        {
        }

        [HttpGet("GetGarantes")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult> GetGarantes()
        {
            try
            {
                var garantes = await _context.Garantes.ToListAsync();

                if (garantes == null || garantes.Count == 0)
                {
                    return NotFound(new { statusCode = StatusCodes.Status200OK, message = "No hay garantes disponibles." });
                }

                return Ok(garantes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpGet("GetGaranteById/{id_garante}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult> GetGaranteById(int id_garante)
        {
            try
            {
                var garante = await _context.Garantes.FindAsync(id_garante);

                if (garante == null)
                {
                    var errorMessage = new { message = $"No se encontró ningún garante con ID {id_garante}" };
                    return NotFound(errorMessage);
                }

                return Ok(garante);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpPost("CrearGarante")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        public async Task<ActionResult> CrearGarante([FromBody] CrearGaranteRequest crearGaranteRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var nuevoGarante = new Garante
                {
                    nombre = crearGaranteRequest.nombre,
                    apellido = crearGaranteRequest.apellido,
                    dni = crearGaranteRequest.dni,
                    garantia = crearGaranteRequest.garantia,
                    fecha_alta = DateOnly.FromDateTime(DateTime.Now)
                };

                _context.Garantes.Add(nuevoGarante);
                await _context.SaveChangesAsync();

                var successMessage = $"Se ha creado un nuevo garante exitosamente";

                return Ok(new { Message = successMessage, GaranteId = nuevoGarante.id_garante });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpPut("EditarGarante/{id_garante}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult> EditarGarante(int id_garante, [FromBody] EditarGaranteRequest editarGaranteRequest)
        {
            try
            {
                var garante = await _context.Garantes.FindAsync(id_garante);

                if (garante == null)
                {
                    var errorMessage = new { statusCode = 404, message = $"No se encontró ningún inquilino con ID {id_garante}" };
                    return NotFound(errorMessage);
                }

                if (!string.IsNullOrEmpty(editarGaranteRequest.nombre))
                {
                    garante.nombre = editarGaranteRequest.nombre;
                }

                if (!string.IsNullOrEmpty(editarGaranteRequest.apellido))
                {
                    garante.apellido = editarGaranteRequest.apellido;
                }

                if (!string.IsNullOrEmpty(editarGaranteRequest.dni))
                {
                    garante.dni = editarGaranteRequest.dni;
                }

                if (!string.IsNullOrEmpty(editarGaranteRequest.garantia))
                {
                    garante.garantia = editarGaranteRequest.garantia;
                }

                await _context.SaveChangesAsync();

                var successMessage = $"Se ha editado un garante con ID {id_garante} exitosamente";
                return Ok(new { statusCode = 200, GaranteId = garante.id_garante, Message = successMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
