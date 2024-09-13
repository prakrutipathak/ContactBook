using System.IdentityModel.Tokens.Jwt;

namespace ClientApplicationContactBook.Infrastructure
{
    public interface IJwtTokenHandler
    {
        JwtSecurityToken ReadJwtToken(string token);
    }
}
