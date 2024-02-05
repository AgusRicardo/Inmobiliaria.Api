﻿using Inmobiliaria.Models;
using Inmobiliaria.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria.Controllers
{
    public class PropietariosController : ApiController
    {
        public PropietariosController(QczbbchrContext context) : base(context)
        {
        }
        [HttpGet("GetPropietarios")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult> GetPropietarios()
        {
            try
            {
                var propietarios = await _context.Propietarios.ToListAsync();
                return Ok(propietarios);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpGet("GetPropietarios/Propietario")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult> GetPropietarioById([FromQuery] string searchTerm)
        {
            try
            {
                string searchTermLower = searchTerm.ToLower();
                var propietarios = await _context.Propietarios
                    .Where(p =>
                        EF.Functions.Like(p.nombre.ToLower(), $"%{searchTermLower}%") ||
                        EF.Functions.Like(p.apellido.ToLower(), $"%{searchTermLower}%") ||
                        EF.Functions.Like(p.dni.ToLower(), $"%{searchTermLower}%"))
                    .ToListAsync();

                if (propietarios.Count == 0)
                {
                    var errorMessage = new { message = $"No se encontraron propietarios con el término de búsqueda '{searchTermLower}'" };
                    return NotFound(errorMessage);
                }

                return Ok(propietarios);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpPost("CrearPropietario")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        public async Task<ActionResult> CrearPropietario([FromBody] CrearPropietarioRequest nuevoPropietarioRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var nuevoPropietario = new Propietario
                {
                    nombre = nuevoPropietarioRequest.nombre,
                    apellido = nuevoPropietarioRequest.apellido,
                    dni = nuevoPropietarioRequest.dni,
                    fecha_alta = DateOnly.FromDateTime(DateTime.Now)
                };

                _context.Propietarios.Add(nuevoPropietario);
                await _context.SaveChangesAsync();

                var successMessage = $"Se ha creado un nuevo propietario exitosamente";

                return Ok(new { Message = successMessage, PersonaId = nuevoPropietario.id_propietario });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpDelete("EliminarPropietario/{id_propietario}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult> EliminarPropietario(int id_propietario)
        {
            try
            {
                var propietario = await _context.Propietarios.FindAsync(id_propietario);

                if (propietario == null)
                {
                    var errorMessage = new { StatusCode = 404, message = $"No se encontró ningun propietario con ID {id_propietario}" };
                    return NotFound(errorMessage);
                }

                _context.Propietarios.Remove(propietario);
                await _context.SaveChangesAsync();

                var successMessage = $"Se ha eliminado el propietario con ID {id_propietario} exitosamente";
                return Ok(new { StatusCode = 200, Message = successMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpPut("EditarPersona/{id_propietario}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult> EditarPersona(int id_propietario, [FromBody] EditarPropietarioRequest editarPropietarioRequest)
        {
            try
            {
                var propietario = await _context.Propietarios.FindAsync(id_propietario);

                if (propietario == null)
                {
                    var errorMessage = new { statusCode = 404, message = $"No se encontró nigun propietario con ID {id_propietario}" };
                    return NotFound(errorMessage);
                }

                if (!string.IsNullOrEmpty(editarPropietarioRequest.nombre))
                {
                    propietario.nombre = editarPropietarioRequest.nombre;
                }

                if (!string.IsNullOrEmpty(editarPropietarioRequest.apellido))
                {
                    propietario.apellido = editarPropietarioRequest.apellido;
                }

                if (!string.IsNullOrEmpty(editarPropietarioRequest.dni))
                {
                    propietario.dni = editarPropietarioRequest.dni;
                }

                await _context.SaveChangesAsync();

                var successMessage = $"Se ha editado el propietario con ID {id_propietario} exitosamente";
                return Ok(new { statusCode = 200, PropietarioId = propietario.id_propietario, Message = successMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
