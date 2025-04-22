// File: Employee.Infrastructure.Tests/Services/AccessTokenServiceTests.cs
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Employee.Core.Entities;
using Employee.Core.Enums;
using Employee.Infrastructure.Services;
using Employee.Infrastructure.Setttings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace EmployeeXUnit.Test.InfrastructureLayer.Services
{
    public class AccessTokenServiceTests
    {
        private readonly JwtSettings _validSettings;
        private readonly AccessTokenService _service;

        public AccessTokenServiceTests()
        {
            _validSettings = new JwtSettings
            {
                Key = "VerySecret_Key_That's_Long_Enough_123!",
                Issuer = "UnitTestIssuer",
                Audience = "UnitTestAudience"
            };

            var optionsMock = new Mock<IOptions<JwtSettings>>();
            optionsMock.Setup(o => o.Value).Returns(_validSettings);

            _service = new AccessTokenService(optionsMock.Object);
        }

        [Fact]
        public async Task GenerateToken_Throws_WhenKeyIsNull()
        {
            // Arrange
            var badSettings = new JwtSettings { Key = null, Issuer = "X", Audience = "Y" };
            var opts = new Mock<IOptions<JwtSettings>>();
            opts.Setup(o => o.Value).Returns(badSettings);
            var svc = new AccessTokenService(opts.Object);
            var employee = new EmployeeEntity { Name = "X", Role = Permissions.Admin, EmployeeId = Guid.NewGuid() };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(
                () => svc.GenerateToken(employee)
            );
            Assert.Equal("JWT secret key is not configured.", ex.Message);
        }

        [Fact]
        public async Task GenerateToken_Throws_WhenKeyIsEmpty()
        {
            // Arrange
            var badSettings = new JwtSettings { Key = "", Issuer = "X", Audience = "Y" };
            var opts = new Mock<IOptions<JwtSettings>>();
            opts.Setup(o => o.Value).Returns(badSettings);
            var svc = new AccessTokenService(opts.Object);
            var employee = new EmployeeEntity { Name = "X", Role = Permissions.Admin, EmployeeId = Guid.NewGuid() };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => svc.GenerateToken(employee)
            );
        }

        [Fact]
        public async Task GenerateToken_Throws_WhenEmployeeIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(
                () => _service.GenerateToken(null)
            );
        }

        [Fact]
        
        public async Task GenerateToken_ReturnsJwtSecurityToken_WithExpectedClaimsAndHeaders()
        {
            // Arrange
            var employee = new EmployeeEntity
            {
                Name = "Alice",
                Role = Permissions.Admin,
                EmployeeId = Guid.NewGuid(),
            };

            // Act
            var beforeUtc = DateTime.UtcNow;
            var token = await _service.GenerateToken(employee);
            var afterUtc = DateTime.UtcNow;

            // Assert: correct type
            Assert.IsType<JwtSecurityToken>(token);

            // Assert: issuer & audience
            Assert.Equal(_validSettings.Issuer, token.Issuer);
            Assert.Single(token.Audiences, _validSettings.Audience);

            // Assert: signing algorithm
            Assert.NotNull(token.SigningCredentials);
            Assert.Equal(SecurityAlgorithms.HmacSha256, token.SigningCredentials.Algorithm);

            // Assert: ValidTo is ~10 minutes from issuance (in UTC)
            Assert.InRange(
                token.ValidTo,
                beforeUtc.AddMinutes(10).AddSeconds(-1),
                afterUtc.AddMinutes(10).AddSeconds(+1)
            );

            // Assert: claims
            var claims = token.Claims.ToList();
            Assert.Contains(claims, c => c.Type == ClaimTypes.Name && c.Value == employee.Name);
            Assert.Contains(claims, c => c.Type == ClaimTypes.Role && c.Value == nameof(Permissions.Admin));
            Assert.Contains(claims, c => c.Type == "EmployeeId" && c.Value == employee.EmployeeId.ToString());
        }



        [Fact]
        public async Task GenerateToken_UsesCorrectSymmetricKeyBytes()
        {
            // Arrange
            var employee = new EmployeeEntity
            {
                Name = "Bob",
                Role = Permissions.Admin,
                EmployeeId = Guid.NewGuid()
            };

            // Act
            var token = await _service.GenerateToken(employee);

            // Extract the raw key from the SigningCredentials
            var keyBytes = ((SymmetricSecurityKey)token.SigningCredentials.Key).Key;

            // Assert: it's exactly the UTF8 of our configured key
            var expected = Encoding.UTF8.GetBytes(_validSettings.Key);
            Assert.Equal(expected, keyBytes);
        }
    }
}
