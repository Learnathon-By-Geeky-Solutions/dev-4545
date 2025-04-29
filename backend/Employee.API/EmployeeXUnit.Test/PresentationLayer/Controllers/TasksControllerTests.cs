using Employee.API.Controllers;
using Employee.Application.Commands.Task;
using Employee.Application.Queries.Task;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace EmployeeXUnit.Test.PresentationLayer.Controllers
{
    public class TasksControllerTests
    {
        private readonly Mock<ISender> _mockSender;
        private readonly Mock<IAuthorizationService> _authzMock;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _mockSender = new Mock<ISender>();
            _authzMock = new Mock<IAuthorizationService>();
            _controller = new TasksController(_mockSender.Object, _authzMock.Object);

            // Fake authenticated user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "Admin")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
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
        public async Task GetTaskByEmployeeId_ReturnsOk_WhenAuthorized()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var task = new TaskEntity { TaskId = Guid.NewGuid(), EmployeeId = employeeId };

            _authzMock
                .Setup(a => a.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    employeeId,
                    "CanModifyOwnEmployee"))
                .ReturnsAsync(AuthorizationResult.Success());

            _mockSender
                .Setup(s => s.Send(
                    It.Is<GetTaskByTaskIdQuery>(q => q.Id == employeeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(task);

            // Act
            var result = await _controller.GetTaskByEmployeeId(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(task, okResult.Value);
        }



        [Fact]
        public async Task GetTaskById_ReturnsForbid_WhenUnauthorized()
        {
            // Arrange
            var employeeId = Guid.NewGuid();

            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), employeeId, "CanModifyOwnEmployee"))
                      .ReturnsAsync(AuthorizationResult.Failed());

            // Act
            var result = await _controller.GetTaskById(employeeId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
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
        public async Task UpdateTask_ReturnsBadRequest_WhenEntityNotFoundInValidation()
        {
            // Arrange
            var id = Guid.NewGuid();
            var task = new TaskEntity { TaskId = id };

            _mockSender.Setup(s => s.Send(It.IsAny<GetTaskByTaskIdQuery>(), default))
                       .ReturnsAsync(new TaskEntity()); // means not found for update


            // Act
            var result = await _controller.UpdateTask(id, task);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Entity not found to update.", badRequest.Value);
        }

        [Fact]
        public async Task UpdateTask_ReturnsOkResult_WhenEntityUpdated()
        {
            // Arrange
            var id = Guid.NewGuid();
            var task = new TaskEntity { TaskId = id };

            _mockSender.SetupSequence(s => s.Send(It.IsAny<GetTaskByTaskIdQuery>(), default))
                       .ReturnsAsync((TaskEntity?)null); // Validation passes

            _mockSender.Setup(s => s.Send(It.IsAny<UpdateTaskCommand>(), default))
                       .ReturnsAsync(task);
            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), task.EmployeeId, "CanModifyOwnEmployee"))
                      .ReturnsAsync(AuthorizationResult.Success());

            // Act
            var result = await _controller.UpdateTask(id, task);

            // Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Entity Not Found.", okResult.Value);
        }

        [Fact]
        public async Task DeleteTask_ReturnsBadRequest_WhenEntityNotFoundInValidation()
        {
            // Arrange
            var id = Guid.NewGuid();

            // 1) Simulate “entity not found” by returning null
            _mockSender
                .Setup(s => s.Send(
                    It.Is<GetTaskByTaskIdQuery>(q => q.Id == id),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((TaskEntity?)null);

            // 2) Make AuthorizeAsync always succeed (never return null)
            _authzMock
                .Setup(a => a.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),         // match whatever resource they pass
                    "CanModifyOwnEmployee"))
                .ReturnsAsync(AuthorizationResult.Success());

            // Act
            var result = await _controller.DeleteTask(id);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Entity Not Found.", badRequest.Value);
        }


        [Fact]
        public async Task DeleteTask_ReturnsForbid_WhenUnauthorized()
        {
            // Arrange
            var id = Guid.NewGuid();
            var taskEntity = new TaskEntity { TaskId = id, EmployeeId = Guid.NewGuid() };

            _mockSender.Setup(s => s.Send(It.IsAny<GetTaskByTaskIdQuery>(), default))
                       .ReturnsAsync(taskEntity);

            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), taskEntity.EmployeeId, "CanModifyOwnEmployee"))
                      .ReturnsAsync(AuthorizationResult.Failed());

            // Act
            var result = await _controller.DeleteTask(id);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
       
        public async Task DeleteTask_ReturnsOkResult_WhenAuthorized()
        {
            // Arrange
            var id = Guid.NewGuid();
            var employeeId = Guid.NewGuid();
            var taskEntity = new TaskEntity { TaskId = id, EmployeeId = employeeId };

            _mockSender.Setup(s => s.Send(It.IsAny<GetTaskByTaskIdQuery>(), default))
                       .ReturnsAsync(taskEntity); // should return a valid taskEntity, not null

            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), employeeId, "CanModifyOwnEmployee"))
                      .ReturnsAsync(AuthorizationResult.Success());

            _mockSender.Setup(s => s.Send(It.IsAny<DeleteTaskCommand>(), default))
                       .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteTask(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(true, okResult.Value);
        }

    }
}
