using Inmobiliaria.Models;
using Inmobiliaria.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Inmobiliaria.Controllers
{
    public class PropiedadesController : ApiController
    {
        public PropiedadesController(QczbbchrContext context) : base(context)
        {
        }
        [HttpGet("GetPropiedades")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult> GetPropiedades()
        {
            try
            {
                var propiedades = await _context.Propiedades
                    .Include(c => c.Propietario)
                    .Select(c => new
                    {
                        c.id_propiedad,
                        Propietario = new
                        {
                            c.Propietario.id_propietario,
                            c.Propietario.nombre,
                            c.Propietario.apellido
                        },
                        c.tipo,
                        c.direccion,
                        c.fecha_alta
                    }).ToListAsync();
                return Ok(propiedades);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpGet("GetPropiedadById/{id_propiedad}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult> GetPropiedadById(int id_propiedad)
        {
            try
            {
                var propiedad = await _context.Propiedades.FindAsync(id_propiedad);

                if (propiedad == null)
                {
                    var errorMessage = new { message = $"No se encontró ningun propiedad con ID {id_propiedad}" };
                    return NotFound(errorMessage);
                }

                return Ok(propiedad);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpPost("CrearPropiedades")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        public async Task<ActionResult> CrearPropiedades([FromBody] CrearPropiedadesRequest nuevaPropiedadRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var nuevaPropiedad = new Propiedad
                {
                    id_propietario = nuevaPropiedadRequest.id_propietario,
                    tipo = nuevaPropiedadRequest.tipo,
                    direccion = nuevaPropiedadRequest.direccion,
                    fecha_alta = DateOnly.FromDateTime(DateTime.Now)
                };

                _context.Propiedades.Add(nuevaPropiedad);
                await _context.SaveChangesAsync();

                var successMessage = $"Se ha creado una nueva propiedad exitosamente";

                return Ok(new { Message = successMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpPut("EditarPropiedad/{id_propiedad}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult> EditarPersona(int id_propiedad, [FromBody] EditarPropiedadRequest editarPropiedadRequest)
        {
            try
            {
                var propiedad = await _context.Propiedades.FindAsync(id_propiedad);

                if (propiedad == null)
                {
                    var errorMessage = new { statusCode = 404, message = $"No se encontró nigun propietario con ID {id_propiedad}" };
                    return NotFound(errorMessage);
                }

                if (editarPropiedadRequest.id_propietario != null)
                {
                    propiedad.id_propietario = editarPropiedadRequest.id_propietario;
                }

                if (!string.IsNullOrEmpty(editarPropiedadRequest.tipo))
                {
                    propiedad.tipo = editarPropiedadRequest.tipo;
                }

                if (!string.IsNullOrEmpty(editarPropiedadRequest.direccion))
                {
                    propiedad.direccion = editarPropiedadRequest.direccion;
                }

                await _context.SaveChangesAsync();

                var successMessage = $"Se ha editado una propiedad con ID {id_propiedad} exitosamente";
                return Ok(new { statusCode = 200, PropiedadId = propiedad.id_propiedad, Message = successMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
