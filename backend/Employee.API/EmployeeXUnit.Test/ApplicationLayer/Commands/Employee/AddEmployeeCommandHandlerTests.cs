using System;
using System.Threading;
using System.Threading.Tasks;
using Employee.Application.Commands.Employee;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Moq;
using Xunit;

namespace EmployeeXUnit.Test.ApplicationLayer.Commands.Employee
{
    public class AddEmployeeCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_AddEmployee_And_Return_AddedEmployee()
        {
            // Arrange
            var employee = new EmployeeEntity
            {
                EmployeeId = Guid.NewGuid(), // Can be set here or by repository
                Name = "Nazmus Sakib",
                Stack = "DotNet",
                Email = "sakib@example.com",
                Salt = "random-salt",
                Password = "hashed-password",
                DateOfJoin = DateTime.UtcNow,
                Role = 0,
                Phone = "017XXXXXXXX"
            };

            var repositoryMock = new Mock<IEmployeeRepository>();
            repositoryMock
                .Setup(repo => repo.AddEmployee(It.IsAny<EmployeeEntity>()))
                .ReturnsAsync((EmployeeEntity e) => e); // Simulate returning the same object

            var handler = new AddEmployeeCommandHandler(repositoryMock.Object);
            var command = new AddEmployeeCommand(employee);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            repositoryMock.Verify(
                repo => repo.AddEmployee(It.Is<EmployeeEntity>(e =>
                    e.Name == employee.Name &&
                    e.Email == employee.Email &&
                    e.Stack == employee.Stack &&
                    e.Phone == employee.Phone &&
                    e.Password == employee.Password &&
                    e.Salt == employee.Salt
                )), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(employee.Email, result.Email);
            Assert.Equal(employee.Name, result.Name);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_RepositoryFails()
        {
            // Arrange
            var employee = new EmployeeEntity
            {
                EmployeeId = Guid.NewGuid(),
                Name = "Nazmus Sakib",
                Stack = "DotNet",
                Email = "sakib@example.com",
                Salt = "random-salt",
                Password = "hashed-password",
                DateOfJoin = DateTime.UtcNow,
                Phone = "017XXXXXXXX"
            };

            var repositoryMock = new Mock<IEmployeeRepository>();
            repositoryMock
                .Setup(repo => repo.AddEmployee(It.IsAny<EmployeeEntity>()))
                .ThrowsAsync(new Exception("Repository error"));

            var handler = new AddEmployeeCommandHandler(repositoryMock.Object);
            var command = new AddEmployeeCommand(employee);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(command, CancellationToken.None));

            Assert.Equal("Repository error", ex.Message);
        }
    }
}
