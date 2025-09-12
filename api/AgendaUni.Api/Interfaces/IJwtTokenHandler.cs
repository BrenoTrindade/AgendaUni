using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AgendaUni.Api.Interfaces
{
    public interface IJwtTokenHandler
    {
        SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor);
        string WriteToken(SecurityToken token);
    }
}