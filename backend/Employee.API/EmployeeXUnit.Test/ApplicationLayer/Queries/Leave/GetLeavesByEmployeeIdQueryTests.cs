using Employee.Application.Queries.Leave;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Moq;

namespace EmployeeXUnit.Test.ApplicationLayer.Queries.Leave
{
    public class GetLeavesByEmployeeIdQueryTests
    {
        [Fact]
        public async Task Handle_ShouldReturnLeave_WhenEmployeeHasLeave()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var expectedLeave = new LeaveEntity { LeaveId = Guid.NewGuid(), EmployeeId = employeeId, Reason = "Sick" };

            var mockRepo = new Mock<ILeaveRepository>();
            mockRepo.Setup(repo => repo.GetLeaveByEmployeeId(employeeId)).ReturnsAsync(expectedLeave);

            var handler = new GetLeavesByEmployeeIdQueryHandler(mockRepo.Object);
            var query = new GetLeavesByEmployeeIdQuery(employeeId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(employeeId, result.EmployeeId);
            Assert.Equal("Sick", result.Reason);
            mockRepo.Verify(repo => repo.GetLeaveByEmployeeId(employeeId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenNoLeaveFoundForEmployee()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var mockRepo = new Mock<ILeaveRepository>();
            mockRepo.Setup(repo => repo.GetLeaveByEmployeeId(employeeId)).ReturnsAsync((LeaveEntity?)null);

            var handler = new GetLeavesByEmployeeIdQueryHandler(mockRepo.Object);
            var query = new GetLeavesByEmployeeIdQuery(employeeId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
            mockRepo.Verify(repo => repo.GetLeaveByEmployeeId(employeeId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var mockRepo = new Mock<ILeaveRepository>();
            mockRepo.Setup(repo => repo.GetLeaveByEmployeeId(employeeId)).ThrowsAsync(new Exception("Data access error"));

            var handler = new GetLeavesByEmployeeIdQueryHandler(mockRepo.Object);
            var query = new GetLeavesByEmployeeIdQuery(employeeId);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));
            mockRepo.Verify(repo => repo.GetLeaveByEmployeeId(employeeId), Times.Once);
        }
    }
}