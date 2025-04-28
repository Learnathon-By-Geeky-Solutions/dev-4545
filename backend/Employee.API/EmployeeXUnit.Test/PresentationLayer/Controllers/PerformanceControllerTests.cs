using Employee.API.Controllers;
using Employee.Application.Commands.Performance;
using Employee.Application.Queries.Performance;
using Employee.Core.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeXUnit.Test.PresentationLayer.Controllers
{
    public class PerformanceControllerTests
    {
        private readonly Mock<ISender> _senderMock;
        private readonly Mock<IAuthorizationService> _authzMock;
        private readonly PerformancesController _controller;

        public PerformanceControllerTests()
        {
            _senderMock = new Mock<ISender>();
            _authzMock = new Mock<IAuthorizationService>();
            _controller = new PerformancesController(_senderMock.Object, _authzMock.Object);
        }

        [Fact]
        public async Task GetPerformances_ShouldReturnOkResult()
        {
            // Arrange
            var performances = new List<PerformanceEntity>
            {
                new PerformanceEntity { Id = Guid.NewGuid(), Rating = "Excellent" }
            };
            _senderMock.Setup(s => s.Send(It.IsAny<GetAllPerformancesQuery>(), default))
                       .ReturnsAsync(performances);

            // Act
            var result = await _controller.GetAllPerformances();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(performances);
        }

        [Fact]
        public async Task GetPerformanceById_ShouldReturnOkResult_WhenPerformanceExists()
        {
            // Arrange
            var performanceId = Guid.NewGuid();
            var performance = new PerformanceEntity { Id = performanceId, Rating = "Good" };

            // Mock the necessary dependencies
            _senderMock.Setup(s => s.Send(It.Is<GetPerformancesByIdQuery>(q => q.EmployeeId == performanceId), default))
                       .ReturnsAsync(performance);

            // Mock the authorization service to allow access
            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), performanceId, "CanModifyOwnEmployee"))
                      .ReturnsAsync(AuthorizationResult.Success());

            // Act
            var result = await _controller.GetPerformancesByEmployeeId(performanceId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(performance);
        }


        [Fact]
        public async Task GetPerformanceById_ShouldReturnForbid_WhenUnauthorized()
        {
            // Arrange
            var performanceId = Guid.NewGuid();

            // Mock failure for authorization check
            _authzMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), performanceId, "CanModifyOwnEmployee"))
                      .ReturnsAsync(AuthorizationResult.Failed());

            // Act
            var result = await _controller.GetPerformancesByEmployeeId(performanceId);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task AddPerformance_ShouldReturnOkResult()
        {
            // Arrange
            var performance = new PerformanceEntity { Id = Guid.NewGuid(), Rating = "Average" };
            _senderMock.Setup(s => s.Send(It.IsAny<AddPerformanceCommand>(), default))
                       .ReturnsAsync(performance);

            // Act
            var result = await _controller.AddPerformanceAsync(performance);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(performance);
        }

        [Fact]
        public async Task UpdatePerformance_ShouldReturnOkResult_WhenPerformanceExists()
        {
            // Arrange
            var performanceId = Guid.NewGuid();
            var performance = new PerformanceEntity { Id = performanceId, Rating = "Updated" };
            _senderMock.Setup(s => s.Send(It.IsAny<UpdatePerformanceCommand>(), default))
                       .ReturnsAsync(performance);

            // Act
            var result = await _controller.UpdatePerformance(performanceId, performance);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(performance);
        }

        [Fact]
        public async Task UpdatePerformance_ShouldReturnBadRequest_WhenPerformanceNotFound()
        {
            // Arrange
            var performanceId = Guid.NewGuid();
            var performance = new PerformanceEntity { Id = performanceId, Rating = "Non-Existent" };
            _senderMock.Setup(s => s.Send(It.IsAny<UpdatePerformanceCommand>(), default))
                .ReturnsAsync((PerformanceEntity?)null);

            // Act
            var result = await _controller.UpdatePerformance(performanceId, performance);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().Be("Not found the entity to update");
        }

        [Fact]
        public async Task DeletePerformance_ShouldReturnOkResult()
        {
            // Arrange
            var performanceId = Guid.NewGuid();
            _senderMock.Setup(s => s.Send(It.IsAny<DeletePerformanceCommand>(), default))
                       .ReturnsAsync(true);

            // Act
            var result = await _controller.DeletePerformance(performanceId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
