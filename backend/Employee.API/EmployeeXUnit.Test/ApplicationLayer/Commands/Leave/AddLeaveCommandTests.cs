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
    public class AddLeaveCommandTests
    {
        [Fact]
        public async Task Handle_ShouldAddLeaveAndReturnLeaveEntity()
        {
            // Arrange
            var mockRepo = new Mock<ILeaveRepository>();
            var newLeave = new LeaveEntity { LeaveId = Guid.NewGuid(), EmployeeId = Guid.NewGuid(), Reason = "Vacation" };

            mockRepo.Setup(repo => repo.AddLeave(It.IsAny<LeaveEntity>())).ReturnsAsync(newLeave);

            var handler = new AddLeaveCommandHandler(mockRepo.Object);
            var command = new AddLeaveCommand(newLeave);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newLeave.LeaveId, result.LeaveId);
            mockRepo.Verify(repo => repo.AddLeave(It.IsAny<LeaveEntity>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenAddFails()
        {
            // Arrange
            var mockRepo = new Mock<ILeaveRepository>();
            mockRepo.Setup(repo => repo.AddLeave(It.IsAny<LeaveEntity>())).ThrowsAsync(new Exception("Failed to add leave"));

            var handler = new AddLeaveCommandHandler(mockRepo.Object);
            var command = new AddLeaveCommand(new LeaveEntity {LeaveId = Guid.NewGuid(), EmployeeId = Guid.NewGuid(), Reason = "Vacation" });

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            mockRepo.Verify(repo => repo.AddLeave(It.IsAny<LeaveEntity>()), Times.Once);
        }
    }
}