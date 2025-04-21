using Employee.Application.Commands.Feature;
using Employee.Core.Interfaces;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeXUnit.Test.ApplicationLayer.Commands.Feature
{
    public class DeleteFeatureCommandTests
    {
        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenFeatureIsDeleted()
        {
            // Arrange
            var featureId = Guid.NewGuid();
            var mockRepo = new Mock<IFeatureRepository>();
            mockRepo.Setup(repo => repo.DeleteFeature(featureId)).ReturnsAsync(true);

            var handler = new DeleteFeatureCommandHandler(mockRepo.Object);
            var command = new DeleteFeatureCommand(featureId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            mockRepo.Verify(repo => repo.DeleteFeature(featureId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenFeatureIsNotDeleted()
        {
            // Arrange
            var featureId = Guid.NewGuid();
            var mockRepo = new Mock<IFeatureRepository>();
            mockRepo.Setup(repo => repo.DeleteFeature(featureId)).ReturnsAsync(false);

            var handler = new DeleteFeatureCommandHandler(mockRepo.Object);
            var command = new DeleteFeatureCommand(featureId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            mockRepo.Verify(repo => repo.DeleteFeature(featureId), Times.Once);
        }
    }
}