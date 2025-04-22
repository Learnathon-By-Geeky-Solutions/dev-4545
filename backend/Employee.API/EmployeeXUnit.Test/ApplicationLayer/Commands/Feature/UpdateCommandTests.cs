using Employee.Application.Commands.Feature;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeXUnit.Test.ApplicationLayer.Commands.Feature
{
    public class UpdateFeatureCommandTests
    {
        [Fact]
        public async Task Handle_ShouldReturnUpdatedFeature_WhenUpdateIsSuccessful()
        {
            // Arrange
            var featureId = Guid.NewGuid();
            var featureToUpdate = new FeatureEntity { FeatureId = featureId, FeatureName = "Updated Feature" };
            var mockRepo = new Mock<IFeatureRepository>();

            mockRepo.Setup(repo => repo.UpdateFeature(featureId, featureToUpdate))
                    .ReturnsAsync(featureToUpdate);

            var handler = new UpdateFeatureCommandHandler(mockRepo.Object);
            var command = new UpdateFeatureCommand(featureId, featureToUpdate);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(featureId, result.FeatureId);
            Assert.Equal("Updated Feature", result.FeatureName);
            mockRepo.Verify(repo => repo.UpdateFeature(featureId, featureToUpdate), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenFeatureUpdateFails()
        {
            // Arrange
            var featureId = Guid.NewGuid();
            var featureToUpdate = new FeatureEntity { FeatureId = featureId, FeatureName = "Feature" };
            var mockRepo = new Mock<IFeatureRepository>();

            mockRepo.Setup(repo => repo.UpdateFeature(featureId, featureToUpdate))
                    .ReturnsAsync((FeatureEntity?)null);

            var handler = new UpdateFeatureCommandHandler(mockRepo.Object);
            var command = new UpdateFeatureCommand(featureId, featureToUpdate);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
            mockRepo.Verify(repo => repo.UpdateFeature(featureId, featureToUpdate), Times.Once);
        }
    }
}