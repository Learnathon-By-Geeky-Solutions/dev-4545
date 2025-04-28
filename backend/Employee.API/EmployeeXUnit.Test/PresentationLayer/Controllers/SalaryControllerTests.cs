using Employee.API.Controllers;
using Employee.Application.Commands.Salary;
using Employee.Application.Queries.Salary;
using Employee.Core.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace EmployeeXUnit.Test.PresentationLayer.Controllers
{
    public class SalaryControllerTests
    {
        private readonly Mock<ISender> _senderMock;
        private readonly Mock<IAuthorizationService> _authzMock;
        private readonly SalaryController _controller;

        public SalaryControllerTests()
        {
            _senderMock = new Mock<ISender>();
            _authzMock = new Mock<IAuthorizationService>();
            _controller = new SalaryController(_senderMock.Object, _authzMock.Object);
        }

        [Fact]
        public async Task InsertSalary_ShouldReturnOkResult_WithInsertedSalary()
        {
            // Arrange
            var salary = new SalaryEntity { SalaryId = Guid.NewGuid(), Amount = 5000 };
            _senderMock.Setup(x => x.Send(It.IsAny<AddSalaryCommand>(), default))
                       .ReturnsAsync(salary);

            // Act
            var result = await _controller.InsertSalary(salary);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(salary);
        }

        [Fact]
        public async Task GetSalaries_ShouldReturnOkResult_WithSalaryList()
        {
            // Arrange
            var salaries = new List<SalaryEntity> { new SalaryEntity { SalaryId = Guid.NewGuid(), Amount = 5000 } };
            _senderMock.Setup(x => x.Send(It.IsAny<GetSalaryQuery>(), default))
                       .ReturnsAsync(salaries);

            // Act
            var result = await _controller.GetSalaries();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(salaries);
        }

        [Fact]
        public async Task GetSalaryByEmpId_ShouldReturnOkResult_WhenAuthorized_AndSalaryExists()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var salary = new SalaryEntity { SalaryId = Guid.NewGuid(), EmployeeId = employeeId, Amount = 7000 };

            _authzMock.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), employeeId, "CanModifyOwnEmployee"))
                      .ReturnsAsync(AuthorizationResult.Success());

            _senderMock.Setup(x => x.Send(It.Is<GetSalariesByEmployeeIdQuery>(q => q.EmployeeId == employeeId), default))
                       .ReturnsAsync(salary);

            // Act
            var result = await _controller.GetSalaryByEmpId(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(salary);
        }

        [Fact]
        public async Task GetSalaryByEmpId_ShouldReturnForbid_WhenAuthorizationFails()
        {
            // Arrange
            var employeeId = Guid.NewGuid();

            _authzMock.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), employeeId, "CanModifyOwnEmployee"))
                      .ReturnsAsync(AuthorizationResult.Failed());

            // Act
            var result = await _controller.GetSalaryByEmpId(employeeId);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task GetSalaryByEmpId_ShouldReturnBadRequest_WhenSalaryNotFound()
        {
            // Arrange
            var employeeId = Guid.NewGuid();

            _authzMock.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), employeeId, "CanModifyOwnEmployee"))
                      .ReturnsAsync(AuthorizationResult.Success());

            _senderMock.Setup(x => x.Send(It.IsAny<GetSalariesByEmployeeIdQuery>(), default))
                       .ReturnsAsync((SalaryEntity?)null);

            // Act
            var result = await _controller.GetSalaryByEmpId(employeeId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().Be("There is no salary updated for the employee.");
        }

        [Fact]
        public async Task UpdateSalaryByEmpId_ShouldReturnOk_WhenSalaryUpdated()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var updatedSalary = new SalaryEntity { SalaryId = Guid.NewGuid(), EmployeeId = employeeId, Amount = 8000 };

            _senderMock.Setup(x => x.Send(It.IsAny<UpdateSalaryCommand>(), default))
                       .ReturnsAsync(updatedSalary);

            // Act
            var result = await _controller.UpdateSalaryByEmpId(employeeId, updatedSalary);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(updatedSalary);
        }

        [Fact]
        public async Task UpdateSalaryByEmpId_ShouldReturnBadRequest_WhenEntityNotFound()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var updatedSalary = new SalaryEntity { SalaryId = Guid.NewGuid(), EmployeeId = employeeId };

            _senderMock.Setup(x => x.Send(It.IsAny<UpdateSalaryCommand>(), default))
                       .ReturnsAsync((SalaryEntity?)null);

            // Act
            var result = await _controller.UpdateSalaryByEmpId(employeeId, updatedSalary);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().Be("Entity not found to update.");
        }

        [Fact]
        public async Task DeleteSalaryByEmpId_ShouldReturnOk_WhenDeleted()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var deleteResult = true;

            _senderMock.Setup(x => x.Send(It.IsAny<DeleteSalaryByEmpIdCommand>(), default))
                       .ReturnsAsync(deleteResult);

            // Act
            var result = await _controller.DeleteSalaryByEmpId(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().Be(true);
        }
    }
}
