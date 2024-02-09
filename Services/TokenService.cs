using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Inmobiliaria.services
{
    public interface ITokenService
    {
        JwtSecurityToken DecodeToken(string token);
    }

    public class TokenService : ITokenService
    {
        public JwtSecurityToken DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadToken(token) as JwtSecurityToken;
        }
    }
}

