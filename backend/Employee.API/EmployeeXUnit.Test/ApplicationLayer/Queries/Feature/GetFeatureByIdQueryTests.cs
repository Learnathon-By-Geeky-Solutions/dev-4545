using Employee.Application.Queries.Feature;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeXUnit.Test.ApplicationLayer.Queries.Feature
{
    public class GetFeatureByIdQueryTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFeatures_WhenEmployeeIdIsValid()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var features = new List<FeatureEntity>
        {
            new FeatureEntity { FeatureId = Guid.NewGuid(), FeatureName = "Feature A" },
            new FeatureEntity { FeatureId = Guid.NewGuid(), FeatureName = "Feature B" }
        };

            var mockRepo = new Mock<IFeatureRepository>();
            mockRepo.Setup(repo => repo.GetFeatureByEmployeeId(employeeId)).ReturnsAsync(features);

            var handler = new GetFeatureByIdQueryHandler(mockRepo.Object);
            var query = new GetFeatureByIdQuery(employeeId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            mockRepo.Verify(repo => repo.GetFeatureByEmployeeId(employeeId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoFeaturesForEmployee()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var mockRepo = new Mock<IFeatureRepository>();
            mockRepo.Setup(repo => repo.GetFeatureByEmployeeId(employeeId))
                    .ReturnsAsync(new List<FeatureEntity>());

            var handler = new GetFeatureByIdQueryHandler(mockRepo.Object);
            var query = new GetFeatureByIdQuery(employeeId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
            mockRepo.Verify(repo => repo.GetFeatureByEmployeeId(employeeId), Times.Once);
        }
    }
}