using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Employee.Application.Commands.Task;
using Employee.Application.Queries.Task;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace EmployeeXUnit.Test.ApplicationLayer
{
    public class TaskHandlerTest
    {
        [Fact]
        public async Task Handle_ReturnsAllTasks_FromRepository()
        {
            // Arrange
            var mockRepo = new Mock<ITasksRepository>();
            var expected = new List<TaskEntity>
            {
                new TaskEntity { TaskId = Guid.NewGuid(),Description = "T1" },
                new TaskEntity { TaskId = Guid.NewGuid(),  Description= "T2" }
            };
            mockRepo.Setup(r => r.GetAllTasks())
                    .ReturnsAsync(expected);

            var handler = new GetAllTasksQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(new GetAllTasksQuery(), CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expected);
            mockRepo.Verify(r => r.GetAllTasks(), Times.Once);
        }
    }

    public class GetTaskByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsTasks_ForGivenEmployeeId()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var mockRepo = new Mock<ITasksRepository>();
            var expected = new List<TaskEntity>
            {
                new TaskEntity { TaskId = Guid.NewGuid(), Description = "E1" }
            };
            mockRepo.Setup(r => r.GetTaskByEmployeeIdAsync(employeeId))
                    .ReturnsAsync(expected);

            var handler = new GetTaskByIdQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(new GetTaskByIdQuery(employeeId), CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expected);
            mockRepo.Verify(r => r.GetTaskByEmployeeIdAsync(employeeId), Times.Once);
        }
    }

    public class GetTaskByTaskIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsTaskEntity_WhenFound()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var expected = new TaskEntity { TaskId = taskId, Description = "Specific" };
            var mockRepo = new Mock<ITasksRepository>();
            mockRepo.Setup(r => r.GetTaskByTaskId(taskId))
                    .ReturnsAsync(expected);

            var handler = new GetProjectByIdHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(new GetTaskByTaskIdQuery(taskId), CancellationToken.None);

            // Assert
            result.Should().Be(expected);
            mockRepo.Verify(r => r.GetTaskByTaskId(taskId), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var mockRepo = new Mock<ITasksRepository>();
            mockRepo.Setup(r => r.GetTaskByTaskId(taskId))
                    .ReturnsAsync((TaskEntity?)null);

            var handler = new GetProjectByIdHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(new GetTaskByTaskIdQuery(taskId), CancellationToken.None);

            // Assert
            result.Should().BeNull();
            mockRepo.Verify(r => r.GetTaskByTaskId(taskId), Times.Once);
        }
    }

    public class AddTaskCommandHandlerTests
    {
        [Fact]
        public async Task Handle_AddsTask_AndReturnsCreatedEntity()
        {
            // Arrange
            var entity = new TaskEntity {TaskId = Guid.NewGuid(), Description = "New" };
            var mockRepo = new Mock<ITasksRepository>();
            mockRepo.Setup(r => r.AddTaskAsync(entity))
                    .ReturnsAsync(entity);

            var handler = new AddTaskCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(new AddTaskCommand(entity), CancellationToken.None);

            // Assert
            result.Should().Be(entity);
            mockRepo.Verify(r => r.AddTaskAsync(entity), Times.Once);
        }
    }

    public class DeleteTaskCommandHandlerTests
    {
        [Fact]
        public async Task Handle_DeletesTask_ReturnsTrue_WhenSuccessful()
        {
            // Arrange
            var id = Guid.NewGuid();
            var mockRepo = new Mock<ITasksRepository>();
            mockRepo.Setup(r => r.DeleteTask(id))
                    .ReturnsAsync(true);

            var handler = new DeleteTaskCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(new DeleteTaskCommand(id), CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            mockRepo.Verify(r => r.DeleteTask(id), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsFalse_WhenDeletionFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var mockRepo = new Mock<ITasksRepository>();
            mockRepo.Setup(r => r.DeleteTask(id))
                    .ReturnsAsync(false);

            var handler = new DeleteTaskCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(new DeleteTaskCommand(id), CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            mockRepo.Verify(r => r.DeleteTask(id), Times.Once);
        }
    }

    public class UpdateTaskCommandHandlerTests
    {
        [Fact]
        public async Task Handle_UpdatesTask_AndReturnsUpdatedEntity()
        {
            // Arrange
            var id = Guid.NewGuid();
            var updated = new TaskEntity {TaskId = id, Description = "Updated" };
            var mockRepo = new Mock<ITasksRepository>();
            mockRepo.Setup(r => r.UpdateTask(id, updated))
                    .ReturnsAsync(updated);

            var handler = new UpdateTaskCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(new UpdateTaskCommand(id, updated), CancellationToken.None);

            // Assert
            result.Should().Be(updated);
            mockRepo.Verify(r => r.UpdateTask(id, updated), Times.Once);
        }
    }
}
