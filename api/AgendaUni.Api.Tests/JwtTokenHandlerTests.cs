using Xunit;
using Moq;
using AgendaUni.Api.Services;
using AgendaUni.Api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using AgendaUni.Api.Interfaces;

namespace AgendaUni.Api.Tests
{
    public class JwtTokenHandlerTests
    {
        [Fact]
        public void CreateToken_ShouldReturnSecurityToken()
        {
            // Arrange
            var handler = new JwtTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "test@example.com") }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes("thisisasecretkeyforjwttokengenerationtest")), SecurityAlgorithms.HmacSha256Signature)
            };

            // Act
            var token = handler.CreateToken(tokenDescriptor);

            // Assert
            Assert.NotNull(token);
            Assert.IsType<JwtSecurityToken>(token);
        }

        [Fact]
        public void WriteToken_ShouldReturnStringToken()
        {
            // Arrange
            var handler = new JwtTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "test@example.com") }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes("thisisasecretkeyforjwttokengenerationtest")), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = handler.CreateToken(tokenDescriptor);

            // Act
            var tokenString = handler.WriteToken(token);

            // Assert
            Assert.False(string.IsNullOrEmpty(tokenString));
        }
    }
}