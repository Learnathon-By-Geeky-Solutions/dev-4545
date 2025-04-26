using Employee.Core.Entities;
using Employee.Infrastructure.Data;
using Employee.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeXUnit.Test.InfrastructureLayer
{
    public class ProjectRepositoryTests
    {
        private static AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Isolated DB per test
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddProjectAsync_Should_Add_Project()
        {
            var dbContext = GetDbContext();
            var repository = new ProjectRepository(dbContext);

            var project = new ProjectEntity
            {
                ProjectName = "Test Project",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                Description = "Test Description",
                Client = "Test Client"
            };

            var result = await repository.AddProjectAsync(project);

            Assert.NotNull(result);
            Assert.Equal("Test Project", result.ProjectName);
            Assert.Single(dbContext.Projects);
        }

        [Fact]
        public async Task GetAllProjects_Should_Return_Projects()
        {
            var dbContext = GetDbContext();
            dbContext.Projects.Add(new ProjectEntity
            {
                ProjectId = Guid.NewGuid(),
                ProjectName = "Project 1",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Client = "Client A"
            });
            dbContext.SaveChanges();

            var repository = new ProjectRepository(dbContext);

            var projects = await repository.GetAllProjects();

            Assert.Single(projects);
        }

        [Fact]
        public async Task UpdateProject_Should_Update_Project()
        {
            var dbContext = GetDbContext();
            var project = new ProjectEntity
            {
                ProjectId = Guid.NewGuid(),
                ProjectName = "Old Name",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
                Client = "Old Client",
                Description = "Old Desc"
            };
            dbContext.Projects.Add(project);
            dbContext.SaveChanges();

            var repository = new ProjectRepository(dbContext);

            var updated = new ProjectEntity
            {
                ProjectName = "New Name",
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(15),
                Client = "New Client",
                Description = "Updated Desc"
            };

            var result = await repository.UpdateProject(project.ProjectId, updated);

            Assert.Equal("New Name", result.ProjectName);
            Assert.Equal("New Client", result.Client);
        }

        [Fact]
        public async Task DeleteProject_Should_Remove_Project_Without_Features()
        {
            var dbContext = GetDbContext();
            var project = new ProjectEntity
            {
                ProjectId = Guid.NewGuid(),
                ProjectName = "Project To Delete",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Client = "Client"
            };
            dbContext.Projects.Add(project);
            dbContext.SaveChanges();

            var repository = new ProjectRepository(dbContext);

            var result = await repository.DeleteProject(project.ProjectId);

            Assert.True(result);
            Assert.Empty(dbContext.Projects);
        }

        [Fact]
        public async Task DeleteProject_Should_Handle_Project_With_Features_And_Tasks()
        {
            var dbContext = GetDbContext();
            var projectId = Guid.NewGuid();
            var featureId = Guid.NewGuid();

            dbContext.Projects.Add(new ProjectEntity
            {
                ProjectId = projectId,
                ProjectName = "Project",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Client = "Client"
            });

            dbContext.Features.Add(new FeatureEntity
            {
                FeatureId = featureId,
                ProjectId = projectId,
                FeatureName = "Feature",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(5),
                Description = "Some desc"
            });

            dbContext.Tasks.Add(new TaskEntity
            {
                TaskId = Guid.NewGuid(),
                FeatureId = featureId,
                EmployeeId = Guid.NewGuid(),
                Description = "Task Desc",
                AssignedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(2),
                AssignedBy = Guid.NewGuid(),
                Status = "Open"
            });

            dbContext.SaveChanges();

            var repository = new ProjectRepository(dbContext);

            var result = await repository.DeleteProject(projectId);

            Assert.True(result);
            Assert.Empty(dbContext.Projects);
            Assert.Empty(dbContext.Features);
            Assert.Empty(dbContext.Tasks);
        }

        [Fact]
        public async Task GetProjectByEmployeeId_Should_Return_Projects()
        {
            var dbContext = GetDbContext();
            var employeeId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var featureId = Guid.NewGuid();

            dbContext.Projects.Add(new ProjectEntity
            {
                ProjectId = projectId,
                ProjectName = "Project X",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
                Description = "Big project",
                Client = "Client Z"
            });

            dbContext.Features.Add(new FeatureEntity
            {
                FeatureId = featureId,
                ProjectId = projectId,
                FeatureName = "Login Module",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(5),
                Description = "Handles auth"
            });

            dbContext.Tasks.Add(new TaskEntity
            {
                TaskId = Guid.NewGuid(),
                FeatureId = featureId,
                EmployeeId = employeeId,
                AssignedBy = Guid.NewGuid(),
                AssignedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(2),
                Description = "Fix bug",
                Status = "In Progress"
            });

            dbContext.SaveChanges();

            var repository = new ProjectRepository(dbContext);

            var result = await repository.GetProjectByEmployeeId(employeeId);

            Assert.Single(result);
            Assert.Equal("Project X", result.First().ProjectName);
        }
    }
}