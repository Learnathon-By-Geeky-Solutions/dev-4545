using System;
using System.Threading;
using System.Threading.Tasks;
using Employee.Application.Commands.Task;
using Employee.Core.Interfaces;
using Moq;
using Xunit;

namespace EmployeeXUnit.Test.ApplicationLayer.Commands.Tasks
{
    public class DeleteTaskCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_DeleteTask_And_Return_True_When_DeletionSucceeds()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock
                .Setup(repo => repo.DeleteTask(taskId))
                .ReturnsAsync(true);

            var handler = new DeleteTaskCommandHandler(repositoryMock.Object);
            var command = new DeleteTaskCommand(taskId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            repositoryMock.Verify(
                repo => repo.DeleteTask(It.Is<Guid>(id => id == taskId)),
                Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_Should_Return_False_When_DeletionFails()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock
                .Setup(repo => repo.DeleteTask(taskId))
                .ReturnsAsync(false);

            var handler = new DeleteTaskCommandHandler(repositoryMock.Object);
            var command = new DeleteTaskCommand(taskId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            repositoryMock.Verify(
                repo => repo.DeleteTask(It.Is<Guid>(id => id == taskId)),
                Times.Once);
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_RepositoryThrows()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock
                .Setup(repo => repo.DeleteTask(taskId))
                .ThrowsAsync(new Exception("DeleteTask failed"));

            var handler = new DeleteTaskCommandHandler(repositoryMock.Object);
            var command = new DeleteTaskCommand(taskId);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(
                () => handler.Handle(command, CancellationToken.None));
            Assert.Equal("DeleteTask failed", ex.Message);
        }
    }
}
