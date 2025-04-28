using Employee.API.Controllers;
using Employee.Application.Commands.Project;
using Employee.Application.Queries.Project;
using Employee.Core.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeXUnit.Test.PresentationLayer.Controllers
{
    public class ProjectsControllerTests
    {
        private readonly Mock<ISender> _senderMock;
        private readonly ProjectsController _controller;

        public ProjectsControllerTests()
        {
            _senderMock = new Mock<ISender>();
            _controller = new ProjectsController(_senderMock.Object);
        }

        [Fact]
        public async Task GetAllProjects_ShouldReturnOkResult()
        {
            // Arrange
            var projects = new List<ProjectEntity>
            {
                new ProjectEntity { ProjectId = Guid.NewGuid(), ProjectName = "Project A", Client = "Client A" }
            };
            _senderMock.Setup(s => s.Send(It.IsAny<GetAllProjectsQuery>(), default))
                       .ReturnsAsync(projects);

            // Act
            var result = await _controller.GetAllProjects();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(projects);
        }

        [Fact]
        public async Task GetProjectById_ShouldReturnOkResult_WithListOfProjects()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var projects = new List<ProjectEntity>
    {
        new ProjectEntity
        {
            ProjectId = Guid.NewGuid(),
            ProjectName = "ERP System",
            Client = "ABC Corp",
            StartDate = DateTime.Now.AddDays(-10),
            EndDate = DateTime.Now.AddMonths(1),
            Description = "An enterprise-level ERP system."
        },
        new ProjectEntity
        {
            ProjectId = Guid.NewGuid(),
            ProjectName = "HR Portal",
            Client = "XYZ Inc",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(2),
            Description = "Internal HR management tool."
        }
    };

            _senderMock.Setup(s => s.Send(It.Is<GetProjectByEmployeeIdQuery>(q => q.EmployeeId == employeeId), default))
                       .ReturnsAsync(projects);

            // Act
            var result = await _controller.GetProjectById(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(projects);
        }


        [Fact]
        public async Task AddProject_ShouldReturnOkResult()
        {
            // Arrange
            var project = new ProjectEntity
            {
                ProjectId = Guid.NewGuid(),
                ProjectName = "New Project",
                Client = "Client B",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                Description = "A description"
            };
            _senderMock.Setup(s => s.Send(It.IsAny<AddProjectCommand>(), default))
                       .ReturnsAsync(project);

            // Act
            var result = await _controller.AddProjectAsync(project);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(project);
        }

        [Fact]
        public async Task UpdateProject_ShouldReturnOkResult_WhenProjectExists()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var project = new ProjectEntity
            {
                ProjectId = projectId,
                ProjectName = "Updated Project",
                Client = "Client C",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(2),
                Description = "Updated description"
            };
            _senderMock.Setup(s => s.Send(It.IsAny<UpdateProjectCommand>(), default))
                       .ReturnsAsync(project);

            // Act
            var result = await _controller.UpdateProject(projectId, project);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(project);
        }

        [Fact]
        public async Task UpdateProject_ShouldReturnBadRequest_WhenProjectNotFound()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var project = new ProjectEntity
            {
                ProjectId = projectId,
                ProjectName = "Non-Existent Project",
                Client = "Non-Existent Client",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                Description = "Non-existent project"
            };
            _senderMock.Setup(s => s.Send(It.IsAny<UpdateProjectCommand>(), default))
                   .ReturnsAsync((ProjectEntity?)null!);


            // Act
            var result = await _controller.UpdateProject(projectId, project);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().Be("Not found the entity to update");
        }

        [Fact]
        public async Task DeleteProject_ShouldReturnOkResult()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            _senderMock.Setup(s => s.Send(It.IsAny<DeleteProjectCommand>(), default))
                       .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProject(projectId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
