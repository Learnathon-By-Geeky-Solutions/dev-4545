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
    public class UpdateEmployeeCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_UpdateEmployee_And_Return_UpdatedEmployee_When_EmployeeExists()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employeeToUpdate = new EmployeeEntity
            {
                EmployeeId = employeeId,
                Name = "Jane Doe",
                Stack = "DotNet",
                Email = "jane.doe@example.com",
                Salt = "some-salt",
                Password = "hashed-password",
                DateOfJoin = DateTime.UtcNow,
                Role=0,
                Phone = "0123456789"
            };

            var updatedEmployee = new EmployeeEntity
            {
                EmployeeId = employeeId,
                Name = "Jane Doe Updated",
                Stack = "DotNet",
                Email = "jane.updated@example.com",
                Salt = "updated-salt",
                Password = "updated-hash",
                DateOfJoin = employeeToUpdate.DateOfJoin,
                Role=employeeToUpdate.Role,
                Phone = "0987654321"
            };

            var repositoryMock = new Mock<IEmployeeRepository>();
            repositoryMock
                .Setup(repo => repo.UpdateEmployee(employeeId, employeeToUpdate))
                .ReturnsAsync(updatedEmployee);

            var handler = new UpdateEmployeeCommandHandler(repositoryMock.Object);
            var command = new UpdateEmployeeCommand(employeeId, employeeToUpdate);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            repositoryMock.Verify(
                repo => repo.UpdateEmployee(
                    It.Is<Guid>(id => id == employeeId),
                    It.Is<EmployeeEntity>(e => e == employeeToUpdate)
                ),
                Times.Once);

            Assert.NotNull(result);
            Assert.Equal(updatedEmployee.EmployeeId, result!.EmployeeId);
            Assert.Equal(updatedEmployee.Name, result.Name);
            Assert.Equal(updatedEmployee.Email, result.Email);
            Assert.Equal(updatedEmployee.Phone, result.Phone);
            Assert.Equal(updatedEmployee.Role, result.Role);
        }

        [Fact]
        public async Task Handle_Should_Return_Null_When_Employee_Not_Found()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employeeToUpdate = new EmployeeEntity
            {
                EmployeeId = employeeId,
                Name = "Jane Doe",
                Stack = "DotNet",
                Email = "jane.doe@example.com",
                Salt = "some-salt",
                Password = "hashed-password",
                Role=0,
                DateOfJoin = DateTime.UtcNow,
                Phone = "0123456789"
            };

            var repositoryMock = new Mock<IEmployeeRepository>();
            repositoryMock
                .Setup(repo => repo.UpdateEmployee(employeeId, employeeToUpdate))
                .ReturnsAsync((EmployeeEntity?)null); // Simulate not found

            var handler = new UpdateEmployeeCommandHandler(repositoryMock.Object);
            var command = new UpdateEmployeeCommand(employeeId, employeeToUpdate);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            repositoryMock.Verify(
                repo => repo.UpdateEmployee(
                    It.Is<Guid>(id => id == employeeId),
                    It.Is<EmployeeEntity>(e => e == employeeToUpdate)
                ),
                Times.Once);

            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_RepositoryThrows()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employeeToUpdate = new EmployeeEntity
            {
                EmployeeId = employeeId,
                Name = "Jane Doe",
                Stack = "DotNet",
                Email = "jane.doe@example.com",
                Salt = "some-salt",
                Password = "hashed-password",
                Role=0,
                DateOfJoin = DateTime.UtcNow,
                Phone = "0123456789"
            };

            var repositoryMock = new Mock<IEmployeeRepository>();
            repositoryMock
                .Setup(repo => repo.UpdateEmployee(employeeId, employeeToUpdate))
                .ThrowsAsync(new Exception("Update failed"));

            var handler = new UpdateEmployeeCommandHandler(repositoryMock.Object);
            var command = new UpdateEmployeeCommand(employeeId, employeeToUpdate);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(command, CancellationToken.None));
            Assert.Equal("Update failed", ex.Message);
        }
    }
}
