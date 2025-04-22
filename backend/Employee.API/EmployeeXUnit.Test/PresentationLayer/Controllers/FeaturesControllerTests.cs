using Employee.API.Controllers;
using Employee.Application.Commands.Feature;
using Employee.Application.Queries.Feature;
using Employee.Core.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeXUnit.Test.PresentationLayer.Controllers
{
    public class FeaturesControllerTests
    {
        private readonly Mock<ISender> _mockSender;
        private readonly FeaturesController _controller;

        public FeaturesControllerTests()
        {
            _mockSender = new Mock<ISender>();
            _controller = new FeaturesController(_mockSender.Object);
        }

        [Fact]
        public async Task GetAllFeatures_ShouldReturn_Ok_With_Features()
        {
            // Arrange
            var features = new List<FeatureEntity>
        {
            new FeatureEntity {FeatureId = Guid.NewGuid(), FeatureName = "Feature1"},
            new FeatureEntity {FeatureId = Guid.NewGuid(), FeatureName = "Feature2"}
        };

            _mockSender.Setup(s => s.Send(It.IsAny<GetAllFeaturesQuery>(), default))
                       .ReturnsAsync(features);

            // Act
            var result = await _controller.GetAllFeatures();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().BeEquivalentTo(features);
        }

        [Fact]
        public async Task GetFeaturesById_ShouldReturnOkWithFeatureList()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var features = new List<FeatureEntity>
            {
                new FeatureEntity
                {
                    FeatureId = Guid.NewGuid(),
                    ProjectId = Guid.NewGuid(),
                    FeatureName = "Dashboard",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(5),
                    Description = "Dashboard feature"
                }
            };

            _mockSender.Setup(s => s.Send(It.Is<GetFeatureByIdQuery>(q => q.EmployeeId == employeeId), default))
                       .ReturnsAsync(features);

            // Act
            var result = await _controller.GetFeaturesById(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(features);
        }

        [Fact]
        public async Task AddFeatureAsync_ShouldReturn_Ok_With_AddedFeature()
        {
            // Arrange
            var feature = new FeatureEntity { FeatureId = Guid.NewGuid(), FeatureName = "NewFeature" };

            _mockSender.Setup(s => s.Send(It.IsAny<AddFeatureCommand>(), default))
                       .ReturnsAsync(feature);

            // Act
            var result = await _controller.AddFeatureAsync(feature);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().BeEquivalentTo(feature);
        }

        [Fact]
        public async Task UpdateFeature_ShouldReturn_Ok_With_UpdatedFeature()
        {
            // Arrange
            var featureId = Guid.NewGuid();
            var updatedFeature = new FeatureEntity { FeatureId = featureId, FeatureName = "UpdatedFeature" };

            _mockSender.Setup(s => s.Send(It.IsAny<UpdateFeatureCommand>(), default))
                       .ReturnsAsync(updatedFeature);

            // Act
            var result = await _controller.UpdateFeature(featureId, updatedFeature);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().BeEquivalentTo(updatedFeature);
        }

        [Fact]
        public async Task DeleteFeature_ShouldReturn_Ok_With_DeletedFeature()
        {
            // Arrange
            var featureId = Guid.NewGuid();
            var deletedFeature = new FeatureEntity { FeatureId = featureId, FeatureName = "DeletedFeature" };

            _mockSender.Setup(s => s.Send(It.IsAny<DeleteFeatureCommand>(), default))
                       .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteFeature(featureId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}