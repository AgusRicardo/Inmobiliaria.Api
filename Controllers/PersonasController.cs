using Inmobiliaria.Models;
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
        public async Task<ActionResult> CrearPersona([FromBody] Persona nuevaPersona)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _context.Personas.Add(nuevaPersona);
                await _context.SaveChangesAsync();

                var successMessage = $"Se ha creado una nueva persona exitosamente";

                return Ok(new { PersonaId = nuevaPersona.PersonaId, Message = successMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

    }
}
