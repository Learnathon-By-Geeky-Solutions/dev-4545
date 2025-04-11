using System;
using System.Threading;
using System.Threading.Tasks;
using Employee.Application.Commands.Task;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Moq;
using Xunit;

namespace EmployeeXUnit.Test.ApplicationLayer.Commands.Tasks
{
    public class AddTaskCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_AddTaskAsync_And_Return_AddedTask()
        {
            // Arrange
            var taskEntity = new TaskEntity
            {
                TaskId = Guid.NewGuid(),
                Description = "Complete unit testing",
                AssignedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(3),
                Status = "Pending",
                AssignedBy = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                FeatureId = Guid.NewGuid()
            };

            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock
                .Setup(repo => repo.AddTaskAsync(It.IsAny<TaskEntity>()))
                .ReturnsAsync((TaskEntity t) => t);

            var handler = new AddTaskCommandHandler(repositoryMock.Object);
            var command = new AddTaskCommand(taskEntity);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            repositoryMock.Verify(
                repo => repo.AddTaskAsync(It.Is<TaskEntity>(t =>
                    t.Description == taskEntity.Description &&
                    t.Status == taskEntity.Status &&
                    t.EmployeeId == taskEntity.EmployeeId
                )),
                Times.Once);

            Assert.NotNull(result);
            Assert.Equal(taskEntity.Description, result.Description);
            Assert.Equal(taskEntity.Status, result.Status);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_RepositoryThrows()
        {
            // Arrange
            var taskEntity = new TaskEntity
            {
                TaskId = Guid.NewGuid(),
                Description = "Complete unit testing",
                AssignedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(3),
                Status = "Pending",
                AssignedBy = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                FeatureId = Guid.NewGuid()
            };

            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock
                .Setup(repo => repo.AddTaskAsync(It.IsAny<TaskEntity>()))
                .ThrowsAsync(new Exception("AddTask failed"));

            var handler = new AddTaskCommandHandler(repositoryMock.Object);
            var command = new AddTaskCommand(taskEntity);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(
                () => handler.Handle(command, CancellationToken.None));
            Assert.Equal("AddTask failed", ex.Message);
        }
    }
}
