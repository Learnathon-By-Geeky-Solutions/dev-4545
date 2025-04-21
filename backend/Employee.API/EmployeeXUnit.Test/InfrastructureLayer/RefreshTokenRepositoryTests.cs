using System.IdentityModel.Tokens.Jwt;
using Employee.Application.Interfaces;
using Employee.Core.Entities;
using Employee.Infrastructure.Data;
using Employee.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EmployeeXUnit.Test.InfrastructureLayer
{
    public class RefreshTokenRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetRefreshTokens_Should_Return_All_Tokens()
        {
            // Arrange
            var dbContext = GetDbContext();
            var tokens = new[]
            {
                new RefreshTokenEntity { TokenId = Guid.NewGuid(), EmployeeId = Guid.NewGuid(), RefreshToken = "t1", RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(5) },
                new RefreshTokenEntity { TokenId = Guid.NewGuid(), EmployeeId = Guid.NewGuid(), RefreshToken = "t2", RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(5) },
            };
            await dbContext.RefreshTokens.AddRangeAsync(tokens);
            await dbContext.SaveChangesAsync();

            var repo = new RefreshTokenRepository(dbContext, Mock.Of<IAccessTokenService>());

            // Act
            var result = await repo.GetRefreshTokens();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, t => t.RefreshToken == "t1");
            Assert.Contains(result, t => t.RefreshToken == "t2");
        }

        [Fact]
        public async Task DeleteToken_Should_Remove_And_Return_True_When_Found()
        {
            // Arrange
            var dbContext = GetDbContext();
            var token = new RefreshTokenEntity
            {
                TokenId = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                RefreshToken = "token",
                RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(5)
            };
            await dbContext.RefreshTokens.AddAsync(token);
            await dbContext.SaveChangesAsync();

            var repo = new RefreshTokenRepository(dbContext, Mock.Of<IAccessTokenService>());

            // Act
            var success = await repo.DeleteToken(token.TokenId);

            // Assert
            Assert.True(success);
            Assert.Empty(dbContext.RefreshTokens);
        }

        [Fact]
        public async Task DeleteToken_Should_Return_False_When_Not_Found()
        {
            // Arrange
            var dbContext = GetDbContext();
            var repo = new RefreshTokenRepository(dbContext, Mock.Of<IAccessTokenService>());

            // Act
            var success = await repo.DeleteToken(Guid.NewGuid());

            // Assert
            Assert.False(success);
        }

        [Fact]
        public async Task GetAccessToken_Should_Return_JwtString_When_Token_Valid()
        {
            // Arrange
            var dbContext = GetDbContext();
            var employeeId = Guid.NewGuid();
            var refresh = new RefreshTokenEntity
            {
                TokenId = Guid.NewGuid(),
                EmployeeId = employeeId,
                RefreshToken = "valid-refresh",
                RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(10)
            };
            var employee = new EmployeeEntity
            {
                EmployeeId = employeeId,
                Name="Sakib",
                Password="13ffhhff",
                Salt="23fhfhfhfhfhfh",
                Stack=".NET",
                Phone= "018@#$$$$$"

            };
            await dbContext.RefreshTokens.AddAsync(refresh);
            await dbContext.Employees.AddAsync(employee);
            await dbContext.SaveChangesAsync();

            // Prepare a fake JWT
            var fakeJwt = new JwtSecurityToken(issuer: "test", audience: "test",
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: null!);
            var expectedJwtString = new JwtSecurityTokenHandler().WriteToken(fakeJwt);

            var mockService = new Mock<IAccessTokenService>();
            mockService
                .Setup(s => s.GenerateToken(It.Is<EmployeeEntity>(u => u.EmployeeId == employeeId)))
                .ReturnsAsync(fakeJwt);

            var repo = new RefreshTokenRepository(dbContext, mockService.Object);

            // Act
            var tokenString = await repo.GetAccessToken("valid-refresh");

            // Assert
            Assert.Equal(expectedJwtString, tokenString);
        }

        [Fact]
        public async Task GetAccessToken_Should_Throw_When_Token_Not_Found()
        {
            // Arrange
            var dbContext = GetDbContext();
            var repo = new RefreshTokenRepository(dbContext, Mock.Of<IAccessTokenService>());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => repo.GetAccessToken("nonexistent"));
        }

        [Fact]
        public async Task GetAccessToken_Should_Throw_When_Token_Expired()
        {
            // Arrange
            var dbContext = GetDbContext();
            var employeeId = Guid.NewGuid();
            var expired = new RefreshTokenEntity
            {
                TokenId = Guid.NewGuid(),
                EmployeeId = employeeId,
                RefreshToken = "expired",
                RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(-5)
            };
            await dbContext.RefreshTokens.AddAsync(expired);
            await dbContext.SaveChangesAsync();

            var repo = new RefreshTokenRepository(dbContext, Mock.Of<IAccessTokenService>());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => repo.GetAccessToken("expired"));
        }
    }
}
