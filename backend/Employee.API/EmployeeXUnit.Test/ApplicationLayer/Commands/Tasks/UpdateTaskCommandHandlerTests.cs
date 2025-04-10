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
    public class UpdateTaskCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_UpdateTask_And_Return_UpdatedTask()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var originalTask = new TaskEntity
            {
                TaskId = taskId,
                Description = "Initial description",
                AssignedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(5),
                Status = "Pending",
                AssignedBy = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                FeatureId = Guid.NewGuid()
            };

            var updatedTask = new TaskEntity
            {
                TaskId = taskId,
                Description = "Updated description",
                AssignedDate = originalTask.AssignedDate,
                DueDate = originalTask.DueDate.AddDays(2),
                Status = "Completed",
                AssignedBy = originalTask.AssignedBy,
                EmployeeId = originalTask.EmployeeId,
                FeatureId = originalTask.FeatureId
            };

            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock
                .Setup(repo => repo.UpdateTask(taskId, originalTask))
                .ReturnsAsync(updatedTask);

            var handler = new UpdateTaskCommandHandler(repositoryMock.Object);
            var command = new UpdateTaskCommand(taskId, originalTask);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            repositoryMock.Verify(
                repo => repo.UpdateTask(
                    It.Is<Guid>(id => id == taskId),
                    It.Is<TaskEntity>(t => t == originalTask)
                ),
                Times.Once);

            Assert.NotNull(result);
            Assert.Equal("Updated description", result.Description);
            Assert.Equal("Completed", result.Status);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_RepositoryThrows()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var taskToUpdate = new TaskEntity
            {
                TaskId = taskId,
                Description = "Initial description",
                AssignedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(5),
                Status = "Pending",
                AssignedBy = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                FeatureId = Guid.NewGuid()
            };

            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock
                .Setup(repo => repo.UpdateTask(taskId, taskToUpdate))
                .ThrowsAsync(new Exception("UpdateTask failed"));

            var handler = new UpdateTaskCommandHandler(repositoryMock.Object);
            var command = new UpdateTaskCommand(taskId, taskToUpdate);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(
                () => handler.Handle(command, CancellationToken.None));
            Assert.Equal("UpdateTask failed", ex.Message);
        }
    }
}
