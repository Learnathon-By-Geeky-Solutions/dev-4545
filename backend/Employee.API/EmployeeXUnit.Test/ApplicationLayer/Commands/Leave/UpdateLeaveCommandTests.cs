using Employee.Application.Commands.Leave;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeXUnit.Test.ApplicationLayer.Commands.Leave
{
    public class UpdateLeaveCommandTests
    {
        [Fact]
        public async Task Handle_ShouldUpdateLeaveAndReturnUpdatedLeave()
        {
            // Arrange
            var mockRepo = new Mock<ILeaveRepository>();
            var employeeId = Guid.NewGuid();
            var leaveToUpdate = new LeaveEntity { LeaveId = Guid.NewGuid(), EmployeeId = employeeId, Reason = "Sick" };

            var updatedLeave = new LeaveEntity { LeaveId = leaveToUpdate.LeaveId, EmployeeId = employeeId, Reason = "Sick (Updated)", StartDate = leaveToUpdate.StartDate, EndDate = leaveToUpdate.EndDate.AddDays(1) };

            mockRepo.Setup(repo => repo.UpdateLeave(employeeId, leaveToUpdate)).ReturnsAsync(updatedLeave);

            var handler = new UpdateLeaveCommandHandler(mockRepo.Object);
            var command = new UpdateLeaveCommand(employeeId, leaveToUpdate);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Sick (Updated)", result.Reason);
            mockRepo.Verify(repo => repo.UpdateLeave(employeeId, leaveToUpdate), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenLeaveNotFoundForUpdate()
        {
            // Arrange
            var mockRepo = new Mock<ILeaveRepository>();
            var employeeId = Guid.NewGuid();
            var leaveToUpdate = new LeaveEntity {LeaveId = Guid.NewGuid(), EmployeeId = employeeId, Reason = "Sick" };

            mockRepo.Setup(repo => repo.UpdateLeave(employeeId, leaveToUpdate)).ReturnsAsync((LeaveEntity?)null);

            var handler = new UpdateLeaveCommandHandler(mockRepo.Object);
            var command = new UpdateLeaveCommand(employeeId, leaveToUpdate);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
            mockRepo.Verify(repo => repo.UpdateLeave(employeeId, leaveToUpdate), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUpdateFails()
        {
            // Arrange
            var mockRepo = new Mock<ILeaveRepository>();
            var employeeId = Guid.NewGuid();
            var leaveToUpdate = new LeaveEntity {LeaveId = Guid.NewGuid(), EmployeeId = employeeId, Reason = "Sick" };

            mockRepo.Setup(repo => repo.UpdateLeave(employeeId, leaveToUpdate)).ThrowsAsync(new Exception("Failed to update leave"));

            var handler = new UpdateLeaveCommandHandler(mockRepo.Object);
            var command = new UpdateLeaveCommand(employeeId, leaveToUpdate);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            mockRepo.Verify(repo => repo.UpdateLeave(employeeId, leaveToUpdate), Times.Once);
        }
    }
}