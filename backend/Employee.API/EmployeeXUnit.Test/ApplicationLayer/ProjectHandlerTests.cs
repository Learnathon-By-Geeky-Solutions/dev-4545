using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Employee.Application.Commands.Project;
using Employee.Application.Queries.Project;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Moq;
using Xunit;

namespace EmployeeXUnit.Test.ApplicationLayer
{
    public class ProjectHandlersTests
    {
        private readonly Mock<IProjectRepository> _repoMock;
        private readonly CancellationToken _ct = CancellationToken.None;

        public ProjectHandlersTests()
        {
            _repoMock = new Mock<IProjectRepository>();
        }

        [Fact]
        public async Task AddProjectCommandHandler_Should_Call_AddProjectAsync_And_Return_Project()
        {
            // Arrange
            var project = new ProjectEntity { ProjectId = Guid.NewGuid(), ProjectName = "Test" };
            _repoMock
                .Setup(r => r.AddProjectAsync(project))
                .ReturnsAsync(project);

            var handler = new AddProjectCommandHandler(_repoMock.Object);
            var command = new AddProjectCommand(project);

            // Act
            var result = await handler.Handle(command, _ct);

            // Assert
            Assert.Equal(project, result);
            _repoMock.Verify(r => r.AddProjectAsync(project), Times.Once);
        }

        [Fact]
        public async Task UpdateProjectCommandHandler_Should_Call_UpdateProject_And_Return_Updated_Project()
        {
            // Arrange
            var id = Guid.NewGuid();
            var project = new ProjectEntity { ProjectId = id, ProjectName = "Updated" };
            _repoMock
                .Setup(r => r.UpdateProject(id, project))
                .ReturnsAsync(project);

            var handler = new UpdateProjectCommandHandler(_repoMock.Object);
            var command = new UpdateProjectCommand(id, project);

            // Act
            var result = await handler.Handle(command, _ct);

            // Assert
            Assert.Equal(project, result);
            _repoMock.Verify(r => r.UpdateProject(id, project), Times.Once);
        }

        [Fact]
        public async Task DeleteProjectCommandHandler_Should_Call_DeleteProject_And_Return_True()
        {
            // Arrange
            var id = Guid.NewGuid();
            _repoMock
                .Setup(r => r.DeleteProject(id))
                .ReturnsAsync(true);

            var handler = new DeleteProjectCommandHandler(_repoMock.Object);
            var command = new DeleteProjectCommand(id);

            // Act
            var result = await handler.Handle(command, _ct);

            // Assert
            Assert.True(result);
            _repoMock.Verify(r => r.DeleteProject(id), Times.Once);
        }

        [Fact]
        public async Task DeleteProjectCommandHandler_Should_Return_False_If_Repo_Returns_False()
        {
            // Arrange
            var id = Guid.NewGuid();
            _repoMock
                .Setup(r => r.DeleteProject(id))
                .ReturnsAsync(false);

            var handler = new DeleteProjectCommandHandler(_repoMock.Object);
            var command = new DeleteProjectCommand(id);

            // Act
            var result = await handler.Handle(command, _ct);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetAllProjectsQueryHandler_Should_Return_All_Projects()
        {
            // Arrange
            var list = new List<ProjectEntity>
            {
                new ProjectEntity { ProjectId = Guid.NewGuid(), ProjectName = "A" },
                new ProjectEntity { ProjectId = Guid.NewGuid(), ProjectName = "B" }
            };
            _repoMock
                .Setup(r => r.GetAllProjects())
                .ReturnsAsync(list);

            var handler = new GetAllProjectsQueryHandler(_repoMock.Object);
            var query = new GetAllProjectsQuery();

            // Act
            var result = await handler.Handle(query, _ct);

            // Assert
            Assert.Same(list, result);
            _repoMock.Verify(r => r.GetAllProjects(), Times.Once);
        }

        [Fact]
        public async Task GetAllProjectsQueryHandler_Should_Handle_Empty_List()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetAllProjects())
                .ReturnsAsync(new List<ProjectEntity>());

            var handler = new GetAllProjectsQueryHandler(_repoMock.Object);

            // Act
            var result = await handler.Handle(new GetAllProjectsQuery(), _ct);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetProjectByIdQueryHandler_Should_Return_Projects_For_Given_Employee()
        {
            // Arrange
            var empId = Guid.NewGuid();
            var list = new List<ProjectEntity>
            {
                new ProjectEntity { ProjectId = Guid.NewGuid(), ProjectName = "X" }
            };
            _repoMock
                .Setup(r => r.GetProjectByEmployeeId(empId))
                .ReturnsAsync(list);

            var handler = new GetProjectByEmployeeIdQueryHandler(_repoMock.Object);
            var query = new GetProjectByEmployeeIdQuery(empId);

            // Act
            var result = await handler.Handle(query, _ct);

            // Assert
            Assert.Equal(list, result);
            _repoMock.Verify(r => r.GetProjectByEmployeeId(empId), Times.Once);
        }

        [Fact]
        public async Task GetProjectByIdQueryHandler_Should_Handle_No_Projects()
        {
            // Arrange
            var empId = Guid.NewGuid();
            _repoMock
                .Setup(r => r.GetProjectByEmployeeId(empId))
                .ReturnsAsync(Array.Empty<ProjectEntity>());

            var handler = new GetProjectByEmployeeIdQueryHandler(_repoMock.Object);

            // Act
            var result = await handler.Handle(new GetProjectByEmployeeIdQuery(empId), _ct);

            // Assert
            Assert.Empty(result);
        }
    }
}
