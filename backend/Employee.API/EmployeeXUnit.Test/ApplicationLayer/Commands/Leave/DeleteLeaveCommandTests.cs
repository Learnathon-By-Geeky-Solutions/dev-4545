using Employee.Application.Commands.Leave;
using Employee.Core.Interfaces;
using Moq;

namespace EmployeeXUnit.Test.ApplicationLayer.Commands.Leave
{
    public class DeleteLeaveCommandTests
    {
        [Fact]
        public async Task Handle_ShouldDeleteLeaveAndReturnTrue()
        {
            // Arrange
            var mockRepo = new Mock<ILeaveRepository>();
            var employeeId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.DeleteLeaveByEmployeeId(employeeId)).ReturnsAsync(true);

            var handler = new DeleteLeaveCommandHandler(mockRepo.Object);
            var command = new DeleteLeaveByEmpIdCommand(employeeId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            mockRepo.Verify(repo => repo.DeleteLeaveByEmployeeId(employeeId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenDeleteFails()
        {
            // Arrange
            var mockRepo = new Mock<ILeaveRepository>();
            var employeeId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.DeleteLeaveByEmployeeId(employeeId)).ReturnsAsync(false);

            var handler = new DeleteLeaveCommandHandler(mockRepo.Object);
            var command = new DeleteLeaveByEmpIdCommand(employeeId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            mockRepo.Verify(repo => repo.DeleteLeaveByEmployeeId(employeeId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenDeleteFails()
        {
            // Arrange
            var mockRepo = new Mock<ILeaveRepository>();
            var employeeId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.DeleteLeaveByEmployeeId(employeeId)).ThrowsAsync(new Exception("Failed to delete leave"));

            var handler = new DeleteLeaveCommandHandler(mockRepo.Object);
            var command = new DeleteLeaveByEmpIdCommand(employeeId);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            mockRepo.Verify(repo => repo.DeleteLeaveByEmployeeId(employeeId), Times.Once);
        }
    }
}