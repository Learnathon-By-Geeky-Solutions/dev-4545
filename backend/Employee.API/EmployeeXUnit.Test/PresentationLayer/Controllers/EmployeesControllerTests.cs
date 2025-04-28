using Employee.API.Controllers;
using Employee.Application.Commands.Employee;
using Employee.Application.Queries.Employee;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace EmployeeXUnit.Test.PresentationLayer.Controllers
{
    public class EmployeesControllerTests
    {
        private readonly Mock<ISender> _senderMock = new();
        private readonly Mock<IAuthorizationService> _authzMock = new();
        private readonly EmployeesController _controller;

        public EmployeesControllerTests()
        {
            _controller = new EmployeesController(_senderMock.Object, _authzMock.Object);

            // Setup a fake User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task GetEmployees_ReturnsOk_WithEmployees()
        {
            // Arrange
            _senderMock.Setup(s => s.Send(It.IsAny<GetEmployeeQuery>(), default))
                .ReturnsAsync(new List<EmployeeEntity>());

            // Act
            var result = await _controller.GetEmployees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<EmployeeEntity>>(okResult.Value);
        }

        [Fact]
        public async Task GetEmployeeById_Authorized_ReturnsOk()
        {
            // Arrange
            var id = Guid.NewGuid();

            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), id, "CanModifyOwnEmployee"))
                .ReturnsAsync(AuthorizationResult.Success());

            _senderMock.Setup(s => s.Send(It.IsAny<GetEmployeeByIdQuery>(), default))
                .ReturnsAsync(new EmployeeEntity());

            // Act
            var result = await _controller.GetEmployeeById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<EmployeeEntity>(okResult.Value);
        }

        [Fact]
        public async Task GetEmployeeById_Unauthorized_ReturnsForbid()
        {
            var id = Guid.NewGuid();

            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), id, "CanModifyOwnEmployee"))
                .ReturnsAsync(AuthorizationResult.Failed());

            var result = await _controller.GetEmployeeById(id);

            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task AddEmployee_ReturnsOk()
        {
            var employee = new EmployeeEntity();

            _senderMock.Setup(s => s.Send(It.IsAny<AddEmployeeCommand>(), default))
                .ReturnsAsync(employee);

            var result = await _controller.AddEmployee(employee);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(employee, okResult.Value);
        }

        [Fact]
        public async Task UpdateEmployee_Authorized_ReturnsOk()
        {
            var id = Guid.NewGuid();
            var employee = new EmployeeEntity();

            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), id, "CanModifyOwnEmployee"))
                .ReturnsAsync(AuthorizationResult.Success());

            _senderMock.Setup(s => s.Send(It.IsAny<UpdateEmployeeCommand>(), default))
                .ReturnsAsync(employee);

            var result = await _controller.UpdateEmployee(id, employee);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(employee, okResult.Value);
        }

        [Fact]
        public async Task UpdateEmployee_Authorized_NotFound()
        {
            var id = Guid.NewGuid();
            var employee = new EmployeeEntity();

            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), id, "CanModifyOwnEmployee"))
                .ReturnsAsync(AuthorizationResult.Success());

            _senderMock.Setup(s => s.Send(It.IsAny<UpdateEmployeeCommand>(), default))
                .ReturnsAsync((EmployeeEntity?)null);

            var result = await _controller.UpdateEmployee(id, employee);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateEmployee_Unauthorized_ReturnsForbid()
        {
            var id = Guid.NewGuid();
            var employee = new EmployeeEntity();

            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), id, "CanModifyOwnEmployee"))
                .ReturnsAsync(AuthorizationResult.Failed());

            var result = await _controller.UpdateEmployee(id, employee);

            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_Authorized_ReturnsNoContent()
        {
            var id = Guid.NewGuid();

            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), id, "CanModifyOwnEmployee"))
                .ReturnsAsync(AuthorizationResult.Success());

            var result = await _controller.DeleteEmployee(id);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_Unauthorized_ReturnsForbid()
        {
            var id = Guid.NewGuid();

            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), id, "CanModifyOwnEmployee"))
                .ReturnsAsync(AuthorizationResult.Failed());

            var result = await _controller.DeleteEmployee(id);

            Assert.IsType<ForbidResult>(result);
        }
    }
}