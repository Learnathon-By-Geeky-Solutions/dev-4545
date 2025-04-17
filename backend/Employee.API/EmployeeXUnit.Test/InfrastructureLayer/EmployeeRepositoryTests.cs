using Employee.Application.Common.Interfaces;
using Employee.Application.Interfaces;
using Employee.Core.Entities;
using Employee.Core.Enums;
using Employee.Infrastructure.Data;
using Employee.Infrastructure.Repositories;
using Employee.Infrastructure.Services;
using Management.Core.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Employee.Infrastructure.Tests
{
    public class EmployeeRepositoryTests
    {
        private DbContextOptions<AppDbContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        private (AppDbContext, Mock<ICacheService>, Mock<IConfiguration>, Mock<IAccessTokenService>) SetupDependencies()
        {
            var context = new AppDbContext(GetInMemoryOptions());
            var cacheServiceMock = new Mock<ICacheService>();
            var configurationMock = new Mock<IConfiguration>();
            var accessTokenServiceMock = new Mock<IAccessTokenService>();
            return (context, cacheServiceMock, configurationMock, accessTokenServiceMock);
        }

        [Fact]
        public async Task GetEmployees_ReturnsAllEmployees()
        {
            // Arrange
            var (context, cacheServiceMock, configurationMock, accessTokenServiceMock) = SetupDependencies();
            var repository = new EmployeeRepository(context, cacheServiceMock.Object, configurationMock.Object, accessTokenServiceMock.Object);

            var employee1 = new EmployeeEntity { EmployeeId = Guid.NewGuid(), Name = "John",Password="123456",Salt="23456dfbfdbcsdcdfcdfcdscfdfd", Email = "john@example.com", Stack = "Developer", Phone = "1234567890", DateOfJoin = DateTime.UtcNow, Role = Permissions.Admin };
            var employee2 = new EmployeeEntity { EmployeeId = Guid.NewGuid(), Name = "Jane", Password = "123456", Salt = "23456dfbfdbfgcsdcdfcdfcdscfdfd", Email = "jane@example.com", Stack = "Manager", Phone = "0987654321", DateOfJoin = DateTime.UtcNow, Role = Permissions.Admin };
            await context.Employees.AddRangeAsync(employee1, employee2);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetEmployees();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, e => e.Email == "john@example.com");
            Assert.Contains(result, e => e.Email == "jane@example.com");
        }

        [Fact]
        public async Task GetEmployeeById_EmployeeInCache_ReturnsFromCache()
        {
            // Arrange
            var (context, cacheServiceMock, configurationMock, accessTokenServiceMock) = SetupDependencies();
            var repository = new EmployeeRepository(context, cacheServiceMock.Object, configurationMock.Object, accessTokenServiceMock.Object);

            var employee = new EmployeeEntity { EmployeeId = Guid.NewGuid(), Name = "John", Email = "john@example.com", Stack = "Developer", Phone = "1234567890", DateOfJoin = DateTime.UtcNow, Role = Permissions.Admin };
            cacheServiceMock.Setup(cs => cs.GetAsync<EmployeeEntity>($"employee-{employee.EmployeeId}"))
                           .ReturnsAsync(employee);

            // Act
            var result = await repository.GetEmployeeById(employee.EmployeeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(employee.Email, result.Email);
            cacheServiceMock.Verify(cs => cs.GetAsync<EmployeeEntity>($"employee-{employee.EmployeeId}"), Times.Once());
        }

        [Fact]
        public async Task GetEmployeeById_EmployeeNotInCache_FetchesFromDatabaseAndCaches()
        {
            // Arrange
            var (context, cacheServiceMock, configurationMock, accessTokenServiceMock) = SetupDependencies();
            var repository = new EmployeeRepository(context, cacheServiceMock.Object, configurationMock.Object, accessTokenServiceMock.Object);

            var employee = new EmployeeEntity { EmployeeId = Guid.NewGuid(), Name = "John",Password="134567888",Salt="eeryfbcbxdsdscfdcfdbcfdcdcdscfdvfdvdvgvgf", Email = "john@example.com", Stack = "Developer", Phone = "1234567890", DateOfJoin = DateTime.UtcNow, Role = Permissions.Admin };
            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();

            cacheServiceMock.Setup(cs => cs.GetAsync<EmployeeEntity>($"employee-{employee.EmployeeId}"))
                           .ReturnsAsync((EmployeeEntity)null);

            // Act
            var result = await repository.GetEmployeeById(employee.EmployeeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(employee.Email, result.Email);

            cacheServiceMock.Verify(cs => cs.SetAsync($"employee-{employee.EmployeeId}", employee, It.Is<TimeSpan>(ts => ts.TotalSeconds == 30)), Times.Once());
        }

        [Fact]
        public async Task GetEmployeeById_NonExistentEmployee_ReturnsNull()
        {
            // Arrange
            var (context, cacheServiceMock, configurationMock, accessTokenServiceMock) = SetupDependencies();
            var repository = new EmployeeRepository(context, cacheServiceMock.Object, configurationMock.Object, accessTokenServiceMock.Object);

            var nonExistentId = Guid.NewGuid();
            cacheServiceMock.Setup(cs => cs.GetAsync<EmployeeEntity>($"employee-{nonExistentId}"))
                           .ReturnsAsync((EmployeeEntity)null);

            // Act
            var result = await repository.GetEmployeeById(nonExistentId);

            // Assert
            Assert.Null(result);
            cacheServiceMock.Verify(cs => cs.SetAsync(It.IsAny<string>(), It.IsAny<EmployeeEntity>(), It.IsAny<TimeSpan>()), Times.Never());
        }

        [Fact]
        public async Task AddEmployee_AddsEmployeeWithHashedPassword()
        {
            // Arrange
            var (context, cacheServiceMock, configurationMock, accessTokenServiceMock) = SetupDependencies();
            var repository = new EmployeeRepository(context, cacheServiceMock.Object, configurationMock.Object, accessTokenServiceMock.Object);

            var newEmployee = new EmployeeEntity
            {
                Name = "New Employee",
                Email = "new@example.com",
                Password = "plainpassword",
                Stack = "Developer",
                Phone = "1234567890",
                DateOfJoin = DateTime.UtcNow,
                Role = Permissions.Admin
            };

            // Act
            var result = await repository.AddEmployee(newEmployee);

            // Assert
            Assert.NotEqual(Guid.Empty, result.EmployeeId);
            Assert.NotNull(result.Salt);
            Assert.NotEqual("plainpassword", result.Password);

            var expectedHash = PasswordHasher.HashPassword("plainpassword", result.Salt);
            Assert.Equal(expectedHash, result.Password);

            var savedEmployee = await context.Employees.FindAsync(result.EmployeeId);
            Assert.NotNull(savedEmployee);
            Assert.Equal(result.Password, savedEmployee.Password);
            Assert.Equal("New Employee", savedEmployee.Name);
        }

        [Fact]
        public async Task UpdateEmployee_ExistingEmployee_UpdatesAndInvalidatesCache()
        {
            // Arrange
            var (context, cacheServiceMock, configurationMock, accessTokenServiceMock) = SetupDependencies();
            var repository = new EmployeeRepository(context, cacheServiceMock.Object, configurationMock.Object, accessTokenServiceMock.Object);

            var employee = new EmployeeEntity
            {
                EmployeeId = Guid.NewGuid(),
                Name = "Old Name",
                Email = "old@example.com",
                Stack = "Developer",
                Phone = "1234567890",
                DateOfJoin = DateTime.UtcNow,
                Role = Permissions.Admin,
                Salt = PasswordHasher.GenerateSalt(),
                Password = PasswordHasher.HashPassword("oldpassword", PasswordHasher.GenerateSalt())
            };
            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();

            var updatedEmployee = new EmployeeEntity
            {
                Name = "New Name",
                Email = "new@example.com",
                Password = "newpassword",
                Stack = "Senior Developer",
                Phone = "0987654321",
                DateOfJoin = DateTime.UtcNow.AddDays(1),
                Role = Permissions.Admin
            };

            // Act
            var result = await repository.UpdateEmployee(employee.EmployeeId, updatedEmployee);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Name", result.Name);
            Assert.Equal("new@example.com", result.Email);

            cacheServiceMock.Verify(cs => cs.RemoveAsync($"employee-{employee.EmployeeId}"), Times.Once());

            var dbEmployee = await context.Employees.FindAsync(employee.EmployeeId);
            Assert.NotNull(dbEmployee);
            Assert.Equal("New Name", dbEmployee.Name);
            Assert.Equal("Senior Developer", dbEmployee.Stack);
            Assert.Equal(Permissions.Admin, dbEmployee.Role);
        }

        [Fact]
        public async Task UpdateEmployee_NonExistentEmployee_ReturnsNull()
        {
            // Arrange
            var (context, cacheServiceMock, configurationMock, accessTokenServiceMock) = SetupDependencies();
            var repository = new EmployeeRepository(context, cacheServiceMock.Object, configurationMock.Object, accessTokenServiceMock.Object);

            var nonExistentId = Guid.NewGuid();
            var updatedEmployee = new EmployeeEntity { Name = "New Name", Email = "new@example.com" };

            // Act
            var result = await repository.UpdateEmployee(nonExistentId, updatedEmployee);

            // Assert
            Assert.Null(result);
            cacheServiceMock.Verify(cs => cs.RemoveAsync(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public async Task DeleteEmployee_ExistingEmployee_DeletesAndInvalidatesCache()
        {
            // Arrange
            var (context, cacheServiceMock, configurationMock, accessTokenServiceMock) = SetupDependencies();
            var repository = new EmployeeRepository(context, cacheServiceMock.Object, configurationMock.Object, accessTokenServiceMock.Object);

            var employee = new EmployeeEntity
            {
                EmployeeId = Guid.NewGuid(),
                Name = "ToDelete",
                Email = "delete@example.com",
                Stack = "Developer",
                Phone = "1234567890",
                DateOfJoin = DateTime.UtcNow,
                Role = Permissions.Admin,
                Salt = PasswordHasher.GenerateSalt(),
                Password = PasswordHasher.HashPassword("password", PasswordHasher.GenerateSalt())
            };
            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.DeleteEmployee(employee.EmployeeId);

            // Assert
            Assert.True(result);

            var dbEmployee = await context.Employees.FindAsync(employee.EmployeeId);
            Assert.Null(dbEmployee);

            cacheServiceMock.Verify(cs => cs.RemoveAsync($"employee-{employee.EmployeeId}"), Times.Once());
        }

        [Fact]
        public async Task DeleteEmployee_NonExistentEmployee_ReturnsFalse()
        {
            // Arrange
            var (context, cacheServiceMock, configurationMock, accessTokenServiceMock) = SetupDependencies();
            var repository = new EmployeeRepository(context, cacheServiceMock.Object, configurationMock.Object, accessTokenServiceMock.Object);

            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await repository.DeleteEmployee(nonExistentId);

            // Assert
            Assert.False(result);
            cacheServiceMock.Verify(cs => cs.RemoveAsync(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public async Task Authenticate_SuccessfulAuthentication_ReturnsAuthenticationResponse()
        {
            // Arrange
            var (context, cacheServiceMock, configurationMock, accessTokenServiceMock) = SetupDependencies();
            var repository = new EmployeeRepository(context, cacheServiceMock.Object, configurationMock.Object, accessTokenServiceMock.Object);

            var employee = new EmployeeEntity
            {
                EmployeeId = Guid.NewGuid(),
                Name = "Test User",
                Email = "test@example.com",
                Role = Permissions.Admin,
                Stack = "Developer",
                Phone = "1234567890",
                DateOfJoin = DateTime.UtcNow
            };
            var salt = PasswordHasher.GenerateSalt();
            employee.Salt = salt;
            employee.Password = PasswordHasher.HashPassword("password", salt);
            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();

            var dummyToken = new JwtSecurityToken();
            accessTokenServiceMock.Setup(s => s.GenerateToken(It.IsAny<EmployeeEntity>())).ReturnsAsync(dummyToken);

            var request = new AuthenticationRequest { Email = "test@example.com", Password = "password" };

            // Act
            var result = await repository.Authenticate(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(employee.EmployeeId, result.Id);
            Assert.Equal("Admin", result.Role);
            Assert.NotNull(result.JwToken);
            Assert.NotNull(result.RefreshToken);

            var refreshTokenEntity = await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.EmployeeId == employee.EmployeeId);
            Assert.NotNull(refreshTokenEntity);
            Assert.Equal(result.RefreshToken, refreshTokenEntity.RefreshToken);
            Assert.True(refreshTokenEntity.RefreshTokenExpiry > DateTime.UtcNow);
        }

        [Fact]
        public async Task Authenticate_UserNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var (context, cacheServiceMock, configurationMock, accessTokenServiceMock) = SetupDependencies();
            var repository = new EmployeeRepository(context, cacheServiceMock.Object, configurationMock.Object, accessTokenServiceMock.Object);

            var request = new AuthenticationRequest { Email = "nonexistent@example.com", Password = "password" };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => repository.Authenticate(request));
        }

        [Fact]
        public async Task Authenticate_IncorrectPassword_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var (context, cacheServiceMock, configurationMock, accessTokenServiceMock) = SetupDependencies();
            var repository = new EmployeeRepository(context, cacheServiceMock.Object, configurationMock.Object, accessTokenServiceMock.Object);

            var employee = new EmployeeEntity
            {
                EmployeeId = Guid.NewGuid(),
                Name = "Test User",
                Email = "test@example.com",
                Stack = "Developer",
                Phone = "1234567890",
                DateOfJoin = DateTime.UtcNow,
                Role = Permissions.Admin
            };
            var salt = PasswordHasher.GenerateSalt();
            employee.Salt = salt;
            employee.Password = PasswordHasher.HashPassword("password", salt);
            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();

            var request = new AuthenticationRequest { Email = "test@example.com", Password = "wrongpassword" };

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => repository.Authenticate(request));
        }
    }
}