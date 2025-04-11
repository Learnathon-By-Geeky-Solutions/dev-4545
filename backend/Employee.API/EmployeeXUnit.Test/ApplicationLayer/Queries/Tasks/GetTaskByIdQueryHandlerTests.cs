using Employee.Application.Queries.Task;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Moq;

namespace EmployeeXUnit.Test.ApplicationLayer.Queries.Tasks
{
    public class GetTaskByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Return_Task_When_Found()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var taskEntity = new TaskEntity
            {
                TaskId = taskId,
                Description = "Review code",
                AssignedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(2),
                Status = "Pending",
                AssignedBy = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                FeatureId = Guid.NewGuid()
            };

            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock
                .Setup(repo => repo.GetTaskByIdAsync(taskId))
                .ReturnsAsync(taskEntity);

            var handler = new GetTaskByIdCommandHandler(repositoryMock.Object);
            var query = new GetTaskByIdQuery(taskId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskEntity.TaskId, result.TaskId);
            Assert.Equal(taskEntity.Description, result.Description);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_RepositoryThrows()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock
                .Setup(repo => repo.GetTaskByIdAsync(taskId))
                .ThrowsAsync(new Exception("GetTaskById failed"));

            var handler = new GetTaskByIdCommandHandler(repositoryMock.Object);
            var query = new GetTaskByIdQuery(taskId);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(
                () => handler.Handle(query, CancellationToken.None));
            Assert.Equal("GetTaskById failed", ex.Message);
        }
    }
}
