using System.Security.Claims;
using Employee.API.Controllers;
using Employee.Application.Commands.Leave;
using Employee.Application.Queries.Leave;
using Employee.Core.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeXUnit.Test.PresentationLayer.Controllers
{
    public class LeaveControllerTests
    {
        private readonly Mock<ISender> _mockSender;
        private readonly Mock<IAuthorizationService> _authzMock;
        private readonly LeaveController _controller;

        public LeaveControllerTests()
        {
            _mockSender = new Mock<ISender>();
            _authzMock = new Mock<IAuthorizationService>();

            // Initialize the controller with mocked dependencies
            _controller = new LeaveController(_mockSender.Object, _authzMock.Object);
        }

        [Fact]
        public async Task InsertLeave_ShouldReturn_Ok_With_InsertedLeave()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var leave = new LeaveEntity { LeaveId = Guid.NewGuid(), Reason = "Sick Leave" };

            // Mock the behavior of the Send method for AddLeaveCommand
            _mockSender.Setup(s => s.Send(It.IsAny<AddLeaveCommand>(), default))
                       .ReturnsAsync(leave);
            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>() ,employeeId, "CanModifyOwnEmployee"))
                .ReturnsAsync(AuthorizationResult.Success());

            // Act
            var result = await _controller.InsertLeave(employeeId, leave);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(leave);
        }

        [Fact]
        public async Task GetLeaves_ShouldReturn_Ok_With_LeavesList()
        {
            // Arrange
            var leaves = new List<LeaveEntity> { new() { LeaveId = Guid.NewGuid(), Reason = "Vacation" } };
            _mockSender.Setup(s => s.Send(It.IsAny<GetLeaveQuery>(), default))
                       .ReturnsAsync(leaves);

            // Act
            var result = await _controller.GetLeaves();

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(leaves);
        }

        [Fact]
        public async Task GetLeaveByEmpId_ShouldReturn_Ok_With_Leave()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var leave = new LeaveEntity { LeaveId = Guid.NewGuid(), Reason = "Medical Leave" };

            // Mock the authorization service to allow access
            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), employeeId, "CanModifyOwnEmployee"))
                      .ReturnsAsync(AuthorizationResult.Success());

            _mockSender.Setup(s => s.Send(It.Is<GetLeavesByEmployeeIdQuery>(q => q.EmployeeId == employeeId), default))
                       .ReturnsAsync(leave);

            // Act
            var result = await _controller.GetLeaveByEmpId(employeeId);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(leave);
        }

        [Fact]
        public async Task UpdateLeave_ShouldReturn_Ok_With_UpdatedLeave()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var updatedLeave = new LeaveEntity { EmployeeId = employeeId, Reason = "Updated Reason" };

            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), employeeId, "CanModifyOwnEmployee"))
                .ReturnsAsync(AuthorizationResult.Success());
            // Mock the behavior of the Send method for UpdateLeaveCommand
            _mockSender.Setup(s => s.Send(It.IsAny<UpdateLeaveCommand>(), default))
                       .ReturnsAsync(updatedLeave);

            // Act
            var result = await _controller.UpdateLeaveByEmpId(employeeId, updatedLeave);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(updatedLeave);
        }

        [Fact]
        public async Task UpdateLeave_ShouldReturn_BadRequest_When_LeaveNotFound()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var updatedLeave = new LeaveEntity { EmployeeId = employeeId, Reason = "Non-Existent Leave" };

            // Simulate not found scenario by returning null from the mock sender
            _mockSender.Setup(s => s.Send(It.IsAny<UpdateLeaveCommand>(), default))
                       .ReturnsAsync((LeaveEntity?)null);
            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), employeeId, "CanModifyOwnEmployee"))
                .ReturnsAsync(AuthorizationResult.Success());

            // Act
            var result = await _controller.UpdateLeaveByEmpId(employeeId, updatedLeave);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().Be("Not found the entity to update.");
        }

        [Fact]
        public async Task DeleteLeaveByEmpId_ShouldReturn_Ok()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            _mockSender.Setup(s => s.Send(It.IsAny<DeleteLeaveByEmpIdCommand>(), default))
                       .ReturnsAsync(true);

            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(),employeeId, "CanModifyOwnEmployee"))
                    .ReturnsAsync(AuthorizationResult.Success());
            // Act
            var result = await _controller.DeleteLeaveByEmpId(employeeId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
