using Xunit;
using Moq;
using AgendaUni.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System;

namespace AgendaUni.Api.Tests
{
    public class JwtValidationServiceTests
    {
        private readonly JwtConfigurationService _jwtConfigurationService;

        public JwtValidationServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"JwtSettings:Secret", "thisisasecretkeyforjwttokengenerationtest"},
                {"JwtSettings:Issuer", "TestIssuer"},
                {"JwtSettings:Audience", "TestAudience"},
                {"JwtSettings:ExpirationInMinutes", "60"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _jwtConfigurationService = new JwtConfigurationService(configuration);
        }

        private string GenerateTestToken(string secret, string issuer, string audience, DateTime expires, DateTime? notBefore = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "test@example.com"),
                    new Claim(ClaimTypes.Role, "User")
                }),
                Expires = expires,
                NotBefore = notBefore ?? DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = issuer,
                Audience = audience
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [Fact]
        public void ValidateToken_WithValidToken_ShouldReturnClaimsPrincipal()
        {
            // Arrange
            var service = new JwtValidationService(_jwtConfigurationService);
            var validToken = GenerateTestToken(
                "thisisasecretkeyforjwttokengenerationtest",
                "TestIssuer",
                "TestAudience",
                DateTime.UtcNow.AddMinutes(5)
            );

            // Act
            var principal = service.ValidateToken(validToken);

            // Assert
            Assert.NotNull(principal);
            Assert.True(principal.Identity.IsAuthenticated);
            Assert.Equal("test@example.com", principal.FindFirst(ClaimTypes.Name)?.Value);
            Assert.Equal("User", principal.FindFirst(ClaimTypes.Role)?.Value);
        }

        [Fact]
        public void ValidateToken_WithInvalidToken_ShouldReturnNull()
        {
            // Arrange
            var service = new JwtValidationService(_jwtConfigurationService);
            var invalidToken = "invalid.token.string";

            // Act
            var principal = service.ValidateToken(invalidToken);

            // Assert
            Assert.Null(principal);
        }

        [Fact]
        public void ValidateToken_WithExpiredToken_ShouldReturnNull()
        {
            // Arrange
            var service = new JwtValidationService(_jwtConfigurationService);
            var expiredToken = GenerateTestToken(
                "thisisasecretkeyforjwttokengenerationtest",
                "TestIssuer",
                "TestAudience",
                DateTime.UtcNow.AddMinutes(-5), // Token expired 5 minutes ago
                DateTime.UtcNow.AddMinutes(-10) // NotBefore 10 minutes ago
            );

            // Act
            var principal = service.ValidateToken(expiredToken);

            // Assert
            Assert.Null(principal);
        }

        [Fact]
        public void ValidateToken_WithWrongSecret_ShouldReturnNull()
        {
            // Arrange
            var service = new JwtValidationService(_jwtConfigurationService);
            var tokenWithWrongSecret = GenerateTestToken(
                "wrongsecretkeyforjwttokengenerationtest", // Wrong secret
                "TestIssuer",
                "TestAudience",
                DateTime.UtcNow.AddMinutes(5)
            );

            // Act
            var principal = service.ValidateToken(tokenWithWrongSecret);

            // Assert
            Assert.Null(principal);
        }

        [Fact]
        public void ValidateToken_WithWrongIssuer_ShouldReturnNull()
        {
            // Arrange
            var service = new JwtValidationService(_jwtConfigurationService);
            var tokenWithWrongIssuer = GenerateTestToken(
                "thisisasecretkeyforjwttokengenerationtest",
                "WrongIssuer", // Wrong issuer
                "TestAudience",
                DateTime.UtcNow.AddMinutes(5)
            );

            // Act
            var principal = service.ValidateToken(tokenWithWrongIssuer);

            // Assert
            Assert.Null(principal);
        }

        [Fact]
        public void ValidateToken_WithWrongAudience_ShouldReturnNull()
        {
            // Arrange
            var service = new JwtValidationService(_jwtConfigurationService);
            var tokenWithWrongAudience = GenerateTestToken(
                "thisisasecretkeyforjwttokengenerationtest",
                "TestIssuer",
                "WrongAudience", // Wrong audience
                DateTime.UtcNow.AddMinutes(5)
            );

            // Act
            var principal = service.ValidateToken(tokenWithWrongAudience);

            // Assert
            Assert.Null(principal);
        }
    }
}