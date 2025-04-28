using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Employee.Application.Common.Authorization;
using Microsoft.AspNetCore.Authorization;
using Xunit;

namespace EmployeeXUnit.Test.ApplicationLayer
{
    public class OwnEmployeeHandlerTests
    {
        private readonly OwnEmployeeHandler _handler = new OwnEmployeeHandler();
        private readonly OwnEmployeeRequirement _requirement = new OwnEmployeeRequirement();

        [Fact]
        public async Task HandleRequirementAsync_UserIsAdmin_Succeeds()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            var user = new ClaimsPrincipal(identity);
            var context = new AuthorizationHandlerContext(
                new[] { _requirement }, user, targetId);

            // Act
            await _handler.HandleAsync(context);

            // Assert
            Assert.True(context.HasSucceeded);
        }

        [Fact]
        public async Task HandleRequirementAsync_UserMatchesOwnId_Succeeds()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, targetId.ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var user = new ClaimsPrincipal(identity);
            var context = new AuthorizationHandlerContext(
                new[] { _requirement }, user, targetId);

            // Act
            await _handler.HandleAsync(context);

            // Assert
            Assert.True(context.HasSucceeded);
        }

        [Fact]
        public async Task HandleRequirementAsync_UserWithDifferentId_DoesNotSucceed()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var user = new ClaimsPrincipal(identity);
            var context = new AuthorizationHandlerContext(
                new[] { _requirement }, user, targetId);

            // Act
            await _handler.HandleAsync(context);

            // Assert
            Assert.False(context.HasSucceeded);
        }

        [Fact]
        public async Task HandleRequirementAsync_MissingNameIdentifierClaim_DoesNotSucceed()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            var identity = new ClaimsIdentity(new Claim[] { }, "TestAuthType");
            var user = new ClaimsPrincipal(identity);
            var context = new AuthorizationHandlerContext(
                new[] { _requirement }, user, targetId);

            // Act
            await _handler.HandleAsync(context);

            // Assert
            Assert.False(context.HasSucceeded);
        }

        [Fact]
        public async Task HandleRequirementAsync_InvalidGuidClaim_DoesNotSucceed()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "not-a-guid") };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var user = new ClaimsPrincipal(identity);
            var context = new AuthorizationHandlerContext(
                new[] { _requirement }, user, targetId);

            // Act
            await _handler.HandleAsync(context);

            // Assert
            Assert.False(context.HasSucceeded);
        }
    }
}
