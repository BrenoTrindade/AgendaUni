using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using AgendaUni.Api.Interfaces;

namespace AgendaUni.Api.Services
{
    public class JwtTokenHandler : IJwtTokenHandler
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public JwtTokenHandler()
        {
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor)
        {
            return _jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
        }

        public string WriteToken(SecurityToken token)
        {
            return _jwtSecurityTokenHandler.WriteToken(token);
        }
    }
}