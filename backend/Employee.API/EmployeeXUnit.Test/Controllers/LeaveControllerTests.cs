using Employee.API.Controllers;
using Employee.Application.Commands.Leave;
using Employee.Application.Queries.Leave;
using Employee.Core.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeXUnit.Test.Controllers
{
    public class LeaveControllerTests
    {
        private readonly Mock<ISender> _mockSender;
        private readonly LeaveController _controller;

        public LeaveControllerTests()
        {
            _mockSender = new Mock<ISender>();
            _controller = new LeaveController(_mockSender.Object);
        }

        [Fact]
        public async Task InsertLeave_ShouldReturn_Ok_With_InsertedLeave()
        {
            var leave = new LeaveEntity { LeaveId = Guid.NewGuid(), Reason = "Sick Leave" };
            _mockSender.Setup(s => s.Send(It.IsAny<AddLeaveCommand>(), default))
                       .ReturnsAsync(leave);

            var result = await _controller.InsertLeave(leave);

            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(leave);
        }

        [Fact]
        public async Task GetLeaves_ShouldReturn_Ok_With_LeavesList()
        {
            var leaves = new List<LeaveEntity> { new() { LeaveId = Guid.NewGuid(), Reason = "Vacation" } };
            _mockSender.Setup(s => s.Send(It.IsAny<GetLeaveQuery>(), default))
                       .ReturnsAsync(leaves);

            var result = await _controller.GetLeaves();

            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(leaves);
        }

        [Fact]
        public async Task GetLeaveByEmpId_ShouldReturn_Ok_With_Leave()
        {
            var employeeId = Guid.NewGuid();
            var leave = new LeaveEntity { LeaveId = Guid.NewGuid(), Reason = "Medical Leave" };

            _mockSender.Setup(s => s.Send(It.Is<GetLeavesByEmployeeIdQuery>(q => q.EmployeeId == employeeId), default))
                       .ReturnsAsync(leave);

            var result = await _controller.GetLeaveByEmpId(employeeId);

            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(leave);
        }

        [Fact]
        public async Task UpdateLeave_ShouldReturn_Ok_With_UpdatedLeave()
        {
            var employeeId = Guid.NewGuid();
            var updatedLeave = new LeaveEntity { EmployeeId = employeeId, Reason = "Updated Reason" };

            _mockSender.Setup(s => s.Send(It.IsAny<UpdateLeaveCommand>(), default))
                       .ReturnsAsync(updatedLeave);

            var result = await _controller.UpdateSalayByEmpId(employeeId, updatedLeave);

            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(updatedLeave);
        }

        [Fact]
        public async Task DeleteLeaveByEmpId_ShouldReturn_Ok()
        {
            var employeeId = Guid.NewGuid();
            _mockSender.Setup(s => s.Send(It.IsAny<DeleteLeaveByEmpIdCommand>(), default))
                       .ReturnsAsync(true);

            var result = await _controller.DeleteLeaveByEmpId(employeeId);

            result.Should().BeOfType<OkObjectResult>();
        }
    }
}