using System;
using System.Threading;
using System.Threading.Tasks;
using Employee.Application.Commands.Employee;
using Employee.Core.Interfaces;
using Moq;
using Xunit;

namespace EmployeeXUnit.Test.ApplicationLayer.Commands.Employee
{
    public class DeleteEmployeeCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_DeleteEmployee_And_Return_True_When_Deletion_Succeeds()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var repositoryMock = new Mock<IEmployeeRepository>();
            repositoryMock
                .Setup(repo => repo.DeleteEmployee(employeeId))
                .ReturnsAsync(true);

            var handler = new DeleteEmployeeCommandHandler(repositoryMock.Object);
            var command = new DeleteEmployeeCommand(employeeId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            repositoryMock.Verify(
                repo => repo.DeleteEmployee(It.Is<Guid>(id => id == employeeId)),
                Times.Once);

            Assert.True(result);
        }

        [Fact]
        public async Task Handle_Should_Return_False_When_Deletion_Fails()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var repositoryMock = new Mock<IEmployeeRepository>();
            repositoryMock
                .Setup(repo => repo.DeleteEmployee(employeeId))
                .ReturnsAsync(false);

            var handler = new DeleteEmployeeCommandHandler(repositoryMock.Object);
            var command = new DeleteEmployeeCommand(employeeId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            repositoryMock.Verify(
                repo => repo.DeleteEmployee(It.Is<Guid>(id => id == employeeId)),
                Times.Once);

            Assert.False(result);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_RepositoryThrows()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var repositoryMock = new Mock<IEmployeeRepository>();
            repositoryMock
                .Setup(repo => repo.DeleteEmployee(employeeId))
                .ThrowsAsync(new Exception("Deletion failed"));

            var handler = new DeleteEmployeeCommandHandler(repositoryMock.Object);
            var command = new DeleteEmployeeCommand(employeeId);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(command, CancellationToken.None));
            Assert.Equal("Deletion failed", ex.Message);
        }
    }
}
