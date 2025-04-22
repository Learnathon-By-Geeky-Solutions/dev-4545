using Employee.Application.Queries.Performance;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Moq;

namespace EmployeeXUnit.Test.ApplicationLayer.Queries.Performance
{
    public class GetPerformanceByIdQueryTests
    {
        [Fact]
        public async Task Handle_ShouldReturnPerformance_WhenFound()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var performance = new PerformanceEntity { Id = Guid.NewGuid(), EmployeeId = employeeId, Rating = "Bad" };

            var mockRepo = new Mock<IPerformanceRepository>();
            mockRepo.Setup(repo => repo.GetPerformancesByEmployeeId(employeeId)).ReturnsAsync(performance);

            var handler = new GetPerformanceByIdQueryHandler(mockRepo.Object);
            var query = new GetPerformancesByIdQuery(employeeId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(employeeId, result.EmployeeId);
            mockRepo.Verify(repo => repo.GetPerformancesByEmployeeId(employeeId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var mockRepo = new Mock<IPerformanceRepository>();
            mockRepo.Setup(repo => repo.GetPerformancesByEmployeeId(employeeId)).ReturnsAsync((PerformanceEntity?)null);

            var handler = new GetPerformanceByIdQueryHandler(mockRepo.Object);
            var query = new GetPerformancesByIdQuery(employeeId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
            mockRepo.Verify(repo => repo.GetPerformancesByEmployeeId(employeeId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var mockRepo = new Mock<IPerformanceRepository>();
            mockRepo.Setup(repo => repo.GetPerformancesByEmployeeId(employeeId)).ThrowsAsync(new Exception("DB Failure"));

            var handler = new GetPerformanceByIdQueryHandler(mockRepo.Object);
            var query = new GetPerformancesByIdQuery(employeeId);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));
            mockRepo.Verify(repo => repo.GetPerformancesByEmployeeId(employeeId), Times.Once);
        }
    }
}