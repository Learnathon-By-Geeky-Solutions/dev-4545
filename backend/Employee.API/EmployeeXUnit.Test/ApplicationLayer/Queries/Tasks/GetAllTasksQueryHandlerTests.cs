using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Employee.Application.Queries.Task;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Moq;
using Xunit;

namespace EmployeeXUnit.Test.ApplicationLayer.Queries.Tasks
{
    public class GetAllTasksQueryHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Return_All_Tasks()
        {
            // Arrange
            var tasks = new List<TaskEntity>
            {
                new TaskEntity
                {
                    TaskId = Guid.NewGuid(),
                    Description = "Task 1",
                    AssignedDate = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddDays(3),
                    Status = "Pending",
                    AssignedBy = Guid.NewGuid(),
                    EmployeeId = Guid.NewGuid(),
                    FeatureId = Guid.NewGuid()
                },
                new TaskEntity
                {
                    TaskId = Guid.NewGuid(),
                    Description = "Task 2",
                    AssignedDate = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddDays(5),
                    Status = "InProgress",
                    AssignedBy = Guid.NewGuid(),
                    EmployeeId = Guid.NewGuid(),
                    FeatureId = Guid.NewGuid()
                }
            };

            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock
                .Setup(repo => repo.GetAllTasks())
                .ReturnsAsync(tasks);

            var handler = new GetAllTasksQueryHandler(repositoryMock.Object);
            var query = new GetAllTasksQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            var resultList = new List<TaskEntity>(result);
            Assert.Equal(2, resultList.Count);
            Assert.Contains(resultList, t => t.Description == "Task 1");
            Assert.Contains(resultList, t => t.Description == "Task 2");
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_RepositoryThrows()
        {
            // Arrange
            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock
                .Setup(repo => repo.GetAllTasks())
                .ThrowsAsync(new Exception("GetAllTasks failed"));

            var handler = new GetAllTasksQueryHandler(repositoryMock.Object);
            var query = new GetAllTasksQuery();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(
                () => handler.Handle(query, CancellationToken.None));
            Assert.Equal("GetAllTasks failed", ex.Message);
        }
    }
}
