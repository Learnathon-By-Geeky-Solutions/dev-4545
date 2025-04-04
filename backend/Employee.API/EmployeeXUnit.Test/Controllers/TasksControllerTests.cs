using Employee.API.Controllers;
using Employee.Application.Commands.Task;
using Employee.Application.Queries.Task;
using Employee.Core.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeXUnit.Test.Controllers
{
    public class TasksControllerTests
    {
        private readonly Mock<ISender> _senderMock;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _senderMock = new Mock<ISender>();
            _controller = new TasksController(_senderMock.Object);
        }

        [Fact]
        public async Task GetAllTasks_ShouldReturnOkResult()
        {
            // Arrange
            var tasks = new List<TaskEntity>
            {
                new TaskEntity { TaskId = Guid.NewGuid(), Description = "Test Task" }
            };

            _senderMock.Setup(x => x.Send(It.IsAny<GetAllTasksQuery>(), default))
                       .ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetAllTasks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(tasks);
        }

        [Fact]
        public async Task GetTaskById_ShouldReturnOkResult()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new TaskEntity { TaskId = taskId, Description = "Specific Task" };

            _senderMock.Setup(x => x.Send(It.Is<GetTaskByIdQuery>(q => q.Id == taskId), default))
                       .ReturnsAsync(task);

            // Act
            var result = await _controller.GetTaskById(taskId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(task);
        }

        [Fact]
        public async Task AddTask_ShouldReturnOkResult()
        {
            // Arrange
            var task = new TaskEntity { TaskId = Guid.NewGuid(), Description = "New Task" };

            _senderMock.Setup(x => x.Send(It.IsAny<AddTaskCommand>(), default))
                       .ReturnsAsync(task);

            // Act
            var result = await _controller.AddTaskAsync(task);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(task);
        }

        [Fact]
        public async Task UpdateTask_ShouldReturnOkResult_WhenTaskExists()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new TaskEntity { TaskId = taskId, Description = "Updated Task" };

            _senderMock.Setup(x => x.Send(It.IsAny<UpdateTaskCommand>(), default))
                       .ReturnsAsync(task);

            // Act
            var result = await _controller.UpdateTask(taskId, task);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(task);
        }

        [Fact]
        public async Task UpdateTask_ShouldReturnBadRequest_WhenTaskNotFound()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new TaskEntity { TaskId = taskId };

            _senderMock.Setup(x => x.Send(It.IsAny<UpdateTaskCommand>(), default))
                       .ReturnsAsync((TaskEntity)null);

            // Act
            var result = await _controller.UpdateTask(taskId, task);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().Be("Entity Not Found to Update.");
        }

        [Fact]
        public async Task DeleteTask_ShouldReturnOkResult()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            _senderMock.Setup(x => x.Send(It.IsAny<DeleteTaskCommand>(), default))
                       .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteTask(taskId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
