using Inmobiliaria.Models;
using Inmobiliaria.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria.Controllers
{
    public class PersonasController : ApiController
    {
        public PersonasController(QczbbchrContext context) : base(context)
        {
        }

        [HttpGet("GetPersonas")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPersonas()
        {
            try
            {   
                var personas = await _context.Personas.ToListAsync();
                return Ok(personas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpGet("GetPersonaById/{personaId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPersonaById(int personaId)
        {
            try
            {
                var persona = await _context.Personas.FindAsync(personaId);

                if (persona == null)
                {
                    var errorMessage = new { message = $"No se encontró ninguna persona con ID {personaId}" };
                    return NotFound(errorMessage);
                }

                return Ok(persona);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpPost("CrearPersona")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CrearPersona([FromBody] CrearPersonaRequest nuevaPersonaRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var nuevaPersona = new Persona
                {
                    Nombre = nuevaPersonaRequest.Nombre,
                    Apellido = nuevaPersonaRequest.Apellido,
                    Dni = nuevaPersonaRequest.Dni
                };

                _context.Personas.Add(nuevaPersona);
                await _context.SaveChangesAsync();

                var successMessage = $"Se ha creado una nueva persona exitosamente";

                return Ok(new { Message = successMessage, PersonaId = nuevaPersona.PersonaId });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpDelete("EliminarPersona/{personaId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> EliminarPersona(int personaId)
        {
            try
            {
                var persona = await _context.Personas.FindAsync(personaId);

                if (persona == null)
                {
                    var errorMessage = new { StatusCode = 404, message = $"No se encontró ninguna persona con ID {personaId}" };
                    return NotFound(errorMessage);
                }

                _context.Personas.Remove(persona);
                await _context.SaveChangesAsync();

                var successMessage = $"Se ha eliminado la persona con ID {personaId} exitosamente";
                return Ok(new { StatusCode = 200, Message = successMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpPut("EditarPersona/{personaId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> EditarPersona(int personaId, [FromBody] EditarPersonaRequest editarPersonaRequest)
        {
            try
            {
                var persona = await _context.Personas.FindAsync(personaId);

                if (persona == null)
                {
                    var errorMessage = new {statusCode = 404, message = $"No se encontró ninguna persona con ID {personaId}" };
                    return NotFound(errorMessage);
                }

                if (!string.IsNullOrEmpty(editarPersonaRequest.Nombre))
                {
                    persona.Nombre = editarPersonaRequest.Nombre;
                }

                if (!string.IsNullOrEmpty(editarPersonaRequest.Apellido))
                {
                    persona.Apellido = editarPersonaRequest.Apellido;
                }

                if (!string.IsNullOrEmpty(editarPersonaRequest.Dni))
                {
                    persona.Dni = editarPersonaRequest.Dni;
                }

                await _context.SaveChangesAsync();

                var successMessage = $"Se ha editado la persona con ID {personaId} exitosamente";
                return Ok(new { statusCode = 200, PersonaId = persona.PersonaId, Message = successMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }


    }
}
