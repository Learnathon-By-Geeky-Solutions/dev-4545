using Employee.Application.Commands.Feature;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeXUnit.Test.ApplicationLayer.Commands.Feature
{
    public class AddFeatureCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidFeature_ReturnsAddedFeature()
        {
            // Arrange
            var feature = new FeatureEntity { FeatureName = "Test Feature" };
            var addedFeature = new FeatureEntity { FeatureId = Guid.NewGuid(), FeatureName = "Test Feature" };
            var mockRepository = new Mock<IFeatureRepository>();
            mockRepository.Setup(r => r.AddFeatureAsync(feature)).ReturnsAsync(addedFeature);
            var handler = new AddFeatureCommandHandler(mockRepository.Object);
            var command = new AddFeatureCommand(feature);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(addedFeature, result);
            mockRepository.Verify(r => r.AddFeatureAsync(feature), Times.Once);
        }

        [Fact]
        public async Task Handle_NullFeature_ThrowsException()
        {
            // Arrange
            var mockRepository = new Mock<IFeatureRepository>();
            mockRepository.Setup(r => r.AddFeatureAsync(null)).ThrowsAsync(new ArgumentNullException("Feature cannot be null"));
            var handler = new AddFeatureCommandHandler(mockRepository.Object);
            var command = new AddFeatureCommand(null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_RepositoryThrows_PropagatesException()
        {
            // Arrange
            var feature = new FeatureEntity { FeatureName = "Test Feature" };
            var mockRepository = new Mock<IFeatureRepository>();
            mockRepository.Setup(r => r.AddFeatureAsync(feature)).ThrowsAsync(new InvalidOperationException("Database error"));
            var handler = new AddFeatureCommandHandler(mockRepository.Object);
            var command = new AddFeatureCommand(feature);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}