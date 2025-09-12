using Xunit;
using Microsoft.Extensions.Configuration;
using AgendaUni.Api.Services;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;

namespace AgendaUni.Api.Tests
{
    public class JwtConfigurationServiceTests
    {
        [Fact]
        public void GetValidationParameters_ShouldReturnCorrectParameters()
        {
            // Arrange
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

            var service = new JwtConfigurationService(configuration);

            // Act
            var validationParameters = service.GetValidationParameters();

            // Assert
            Assert.True(validationParameters.ValidateIssuerSigningKey);
            Assert.NotNull(validationParameters.IssuerSigningKey);
            Assert.True(validationParameters.ValidateIssuer);
            Assert.Equal("TestIssuer", validationParameters.ValidIssuer);
            Assert.True(validationParameters.ValidateAudience);
            Assert.Equal("TestAudience", validationParameters.ValidAudience);
            Assert.True(validationParameters.ValidateLifetime);
            Assert.Equal(TimeSpan.Zero, validationParameters.ClockSkew);
        }
    }
}