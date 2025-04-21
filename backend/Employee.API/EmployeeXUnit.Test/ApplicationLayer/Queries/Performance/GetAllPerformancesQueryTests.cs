using Employee.Application.Queries.Performance;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeXUnit.Test.ApplicationLayer.Queries.Performance
{
    public class GetAllPerformancesQueryTests
    {
        [Fact]
        public async Task Handle_ShouldReturnListOfPerformances()
        {
            // Arrange
            var mockRepo = new Mock<IPerformanceRepository>();
            var performances = new List<PerformanceEntity>
        {
            new() { Id = Guid.NewGuid(), EmployeeId = Guid.NewGuid(), Rating = "good" },
            new() { Id = Guid.NewGuid(), EmployeeId = Guid.NewGuid(), Rating = "good" }
        };

            mockRepo.Setup(repo => repo.GetAllPerformances()).ReturnsAsync(performances);

            var handler = new GetAllFeaturesQueryHandler(mockRepo.Object);
            var query = new GetAllPerformancesQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<PerformanceEntity>)result).Count);
            mockRepo.Verify(repo => repo.GetAllPerformances(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoPerformancesFound()
        {
            // Arrange
            var mockRepo = new Mock<IPerformanceRepository>();
            mockRepo.Setup(repo => repo.GetAllPerformances()).ReturnsAsync(new List<PerformanceEntity>());

            var handler = new GetAllFeaturesQueryHandler(mockRepo.Object);
            var query = new GetAllPerformancesQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
            mockRepo.Verify(repo => repo.GetAllPerformances(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var mockRepo = new Mock<IPerformanceRepository>();
            mockRepo.Setup(repo => repo.GetAllPerformances()).ThrowsAsync(new Exception("Database error"));

            var handler = new GetAllFeaturesQueryHandler(mockRepo.Object);
            var query = new GetAllPerformancesQuery();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));
            mockRepo.Verify(repo => repo.GetAllPerformances(), Times.Once);
        }
    }
}