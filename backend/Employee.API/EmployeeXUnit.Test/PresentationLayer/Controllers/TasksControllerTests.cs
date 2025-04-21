using Employee.API.Controllers;
using Employee.Application.Commands.Task;
using Employee.Application.Queries.Task;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EmployeeXUnit.Test.PresentationLayer.Controllers
{
    public class TasksControllerTests
    {
        private readonly Mock<ISender> _mockSender;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _mockSender = new Mock<ISender>();
            _controller = new TasksController(_mockSender.Object);
        }

        [Fact]
        public async Task GetAllTasks_ReturnsOkResult_WithTaskList()
        {
            // Arrange
            var tasks = new List<TaskEntity> { new TaskEntity { TaskId = Guid.NewGuid() } };
            _mockSender.Setup(s => s.Send(It.IsAny<GetAllTasksQuery>(), default))
                       .ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetAllTasks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(tasks, okResult.Value);
        }

        [Fact]
        public async Task GetTaskById_ReturnsOkResult_WithTaskList()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var tasks = new List<TaskEntity>
        {
            new TaskEntity { TaskId = Guid.NewGuid(), EmployeeId = employeeId },
            new TaskEntity { TaskId = Guid.NewGuid(), EmployeeId = employeeId }
        };
            _mockSender.Setup(s => s.Send(It.Is<GetTaskByIdQuery>(q => q.Id == employeeId), default))
                       .ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetTaskById(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(tasks, okResult.Value);
        }

        [Fact]
        public async Task AddTaskAsync_ReturnsOkResult_WithCreatedTask()
        {
            // Arrange
            var task = new TaskEntity { TaskId = Guid.NewGuid() };
            _mockSender.Setup(s => s.Send(It.IsAny<AddTaskCommand>(), default))
                       .ReturnsAsync(task);

            // Act
            var result = await _controller.AddTaskAsync(task);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(task, okResult.Value);
        }

        [Fact]
        public async Task UpdateTask_ReturnsOkResult_WhenEntityExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var task = new TaskEntity { TaskId = id };
            _mockSender.Setup(s => s.Send(It.IsAny<UpdateTaskCommand>(), default))
                       .ReturnsAsync(task);

            // Act
            var result = await _controller.UpdateTask(id, task);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(task, okResult.Value);
        }

        [Fact]
        public async Task UpdateTask_ReturnsBadRequest_WhenEntityNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var task = new TaskEntity { TaskId = id };
            _mockSender.Setup(s => s.Send(It.IsAny<UpdateTaskCommand>(), default))
                       .ReturnsAsync((TaskEntity?)null);

            // Act
            var result = await _controller.UpdateTask(id, task);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Entity Not Found to Update.", badRequest.Value);
        }

        [Fact]
        public async Task DeleteTask_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var response = true;
            _mockSender.Setup(s => s.Send(It.IsAny<DeleteTaskCommand>(), default))
                       .ReturnsAsync(response);

            // Act
            var result = await _controller.DeleteTask(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response, okResult.Value);
        }
    }
}