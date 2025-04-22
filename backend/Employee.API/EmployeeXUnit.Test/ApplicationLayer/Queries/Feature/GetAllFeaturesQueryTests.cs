using Employee.Application.Queries.Feature;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeXUnit.Test.ApplicationLayer.Queries.Feature
{
    public class GetAllFeaturesQueryTests
    {
        [Fact]
        public async Task Handle_ShouldReturnAllFeatures()
        {
            // Arrange
            var features = new List<FeatureEntity>
        {
            new FeatureEntity { FeatureId = Guid.NewGuid(), FeatureName = "Feature 1" },
            new FeatureEntity { FeatureId = Guid.NewGuid(), FeatureName = "Feature 2" }
        };

            var mockRepo = new Mock<IFeatureRepository>();
            mockRepo.Setup(repo => repo.GetAllFeatures()).ReturnsAsync(features);

            var handler = new GetAllFeaturesQueryHandler(mockRepo.Object);
            var query = new GetAllFeaturesQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            mockRepo.Verify(repo => repo.GetAllFeatures(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoFeaturesExist()
        {
            // Arrange
            var mockRepo = new Mock<IFeatureRepository>();
            mockRepo.Setup(repo => repo.GetAllFeatures()).ReturnsAsync(new List<FeatureEntity>());

            var handler = new GetAllFeaturesQueryHandler(mockRepo.Object);
            var query = new GetAllFeaturesQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
            mockRepo.Verify(repo => repo.GetAllFeatures(), Times.Once);
        }
    }
}