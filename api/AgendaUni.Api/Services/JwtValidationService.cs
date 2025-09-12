using AgendaUni.Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AgendaUni.Api.Services
{
    public class JwtValidationService
    {
        private readonly TokenValidationParameters _tokenValidationParameters;

        public JwtValidationService(JwtConfigurationService jwtConfigurationService)
        {
            _tokenValidationParameters = jwtConfigurationService.GetValidationParameters();
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}