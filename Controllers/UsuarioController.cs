using Inmobiliaria.Controllers;
using Inmobiliaria.Models;
using Inmobiliaria.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class UsuarioController : ApiController
{
    private readonly IConfiguration _configuration;

    public UsuarioController(QczbbchrContext context, IConfiguration configuration) : base(context)
    {
        _configuration = configuration;
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Login([FromBody] LoginRequest optData)
    {
        try
        {
            string email = optData.Email;
            string password = optData.Password;

            //Usuario usuarioEntity = _context.Usuarios.FirstOrDefault(x => x.Email.ToLower() == email.ToLower() && x.Password == password);
            Usuario usuarioEntity = _context.Usuarios
            .Include(u => u.Inmobiliaria) // Incluir la relación con la inmobiliaria
            .FirstOrDefault(x => x.Email.ToLower() == email.ToLower() && x.Password == password);

            if (usuarioEntity != null)
            {
                var jwt = _configuration.GetSection("Bearer").Get<Jwt>();

                var claims = new[]
                {
                    new Claim("user_id", usuarioEntity.User_id.ToString()),
                    new Claim("mail", usuarioEntity.Email),
                    new Claim("rol", usuarioEntity.id_rol),
                    new Claim("inmobiliaria_id", usuarioEntity.inmobiliaria_id.ToString()),
                    new Claim("inmobiliaria_nombre", usuarioEntity.Inmobiliaria.nombre),
                    new Claim("inmobiliaria_email", usuarioEntity.Inmobiliaria.email),
                    new Claim("inmobiliaria_direccion", usuarioEntity.Inmobiliaria.direccion)
                    // Podría almacenar mas datos en el token
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Bearer:Key").Value));
                var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60), // El token expira después de 1h
                    issuer: _configuration.GetSection("Bearer:Issuer").Value,
                    audience: _configuration.GetSection("Bearer:Audience").Value,
                    signingCredentials: singIn
                );

                return Ok(new
                {
                    success = true,
                    mensaje = "Inicio de sesión exitoso",
                    result = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            else
            {
                return StatusCode(400, new { success = false, mensaje = "Credenciales erróneas" });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, mensaje = "Error interno del servidor" });
        }
    }
}
