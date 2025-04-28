using Employee.API.Controllers;
using Employee.Application.Commands.Feature;
using Employee.Application.Queries.Feature;
using Employee.Application.Queries.Task;
using Employee.Core.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace EmployeeXUnit.Test.PresentationLayer.Controllers
{
    public class FeaturesControllerTests
    {
        private readonly Mock<ISender> _mockSender;
        private readonly Mock<IAuthorizationService> _authzMock;
        private readonly FeaturesController _controller;

        public FeaturesControllerTests()
        {
            _mockSender = new Mock<ISender>();
            _authzMock = new Mock<IAuthorizationService>();
            _controller = new FeaturesController(_mockSender.Object, _authzMock.Object);

            // Setup dummy User for authorization
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "Admin")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task GetAllFeatures_ShouldReturn_Ok_With_FeatureList()
        {
            // Arrange
            var features = new List<FeatureEntity>
            {
                new FeatureEntity { FeatureId = Guid.NewGuid(), FeatureName = "Feature1" },
                new FeatureEntity { FeatureId = Guid.NewGuid(), FeatureName = "Feature2" }
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
        public async Task GetFeaturesById_ShouldReturn_Ok_When_Authorized()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var features = new List<FeatureEntity>
            {
                new FeatureEntity { FeatureId = Guid.NewGuid(), FeatureName = "FeatureA" }
            };

            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), employeeId, "CanModifyOwnEmployee"))
                      .ReturnsAsync(AuthorizationResult.Success());

            _mockSender.Setup(s => s.Send(It.Is<GetFeatureByIdQuery>(q => q.EmployeeId == employeeId), default))
                       .ReturnsAsync(features);

            // Act
            var result = await _controller.GetFeaturesById(employeeId);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().BeEquivalentTo(features);
        }

        [Fact]
        public async Task GetFeaturesById_ShouldReturn_Forbid_When_Unauthorized()
        {
            // Arrange
            var employeeId = Guid.NewGuid();

            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), employeeId, "CanModifyOwnEmployee"))
                      .ReturnsAsync(AuthorizationResult.Failed());

            // Act
            var result = await _controller.GetFeaturesById(employeeId);

            // Assert
            result.Should().BeOfType<ForbidResult>();
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
        public async Task UpdateFeature_ShouldReturn_Ok_When_Authorized_And_Updated()
        {
            // Arrange
            var id = Guid.NewGuid();
            var feature = new FeatureEntity { FeatureId = id, FeatureName = "UpdatedFeature" };
            var taskEntity = new TaskEntity { EmployeeId = Guid.NewGuid() };

            _mockSender.Setup(s => s.Send(It.IsAny<GetTaskByTaskIdQuery>(), default))
                       .ReturnsAsync(taskEntity);

            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), taskEntity.EmployeeId, "CanModifyOwnEmployee"))
                      .ReturnsAsync(AuthorizationResult.Success());

            _mockSender.Setup(s => s.Send(It.IsAny<UpdateFeatureCommand>(), default))
                       .ReturnsAsync(feature);

            // Act
            var result = await _controller.UpdateFeature(id, feature);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().BeEquivalentTo(feature);
        }

        [Fact]
        public async Task UpdateFeature_ShouldReturn_BadRequest_When_EntityNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var feature = new FeatureEntity { FeatureId = id, FeatureName = "MissingFeature" };
            var taskEntity = new TaskEntity { EmployeeId = Guid.NewGuid() };

            _mockSender.Setup(s => s.Send(It.IsAny<GetTaskByTaskIdQuery>(), default))
                       .ReturnsAsync(taskEntity);

            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), taskEntity.EmployeeId, "CanModifyOwnEmployee"))
                      .ReturnsAsync(AuthorizationResult.Success());

            _mockSender.Setup(s => s.Send(It.IsAny<UpdateFeatureCommand>(), default))
                       .ReturnsAsync((FeatureEntity?)null);

            // Act
            var result = await _controller.UpdateFeature(id, feature);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                  .Which.Value.Should().Be("Entity Not Found.");
        }

        [Fact]
        public async Task UpdateFeature_ShouldReturn_Forbid_When_Unauthorized()
        {
            // Arrange
            var id = Guid.NewGuid();
            var feature = new FeatureEntity { FeatureId = id, FeatureName = "UnauthorizedFeature" };
            var taskEntity = new TaskEntity { EmployeeId = Guid.NewGuid() };

            _mockSender.Setup(s => s.Send(It.IsAny<GetTaskByTaskIdQuery>(), default))
                       .ReturnsAsync(taskEntity);

            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), taskEntity.EmployeeId, "CanModifyOwnEmployee"))
                      .ReturnsAsync(AuthorizationResult.Failed());

            // Act
            var result = await _controller.UpdateFeature(id, feature);

            // Assert
            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task DeleteFeature_ShouldReturn_Ok_When_Deleted()
        {
            // Arrange
            var featureId = Guid.NewGuid();
            var deletedFeature = new FeatureEntity { FeatureId = featureId, FeatureName = "DeletedFeature" };

            _mockSender.Setup(s => s.Send(It.IsAny<DeleteFeatureCommand>(), default))
                       .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteFeature(featureId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
