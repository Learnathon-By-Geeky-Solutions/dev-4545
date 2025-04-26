using Employee.Core.Entities;
using Employee.Infrastructure.Data;
using Employee.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeXUnit.Test.InfrastructureLayer
{
    public class TaskRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetAllTasks_Should_Return_All_Tasks()
        {
            // Arrange
            var dbContext = GetDbContext();
            var tasks = new[]
            {
                new TaskEntity { TaskId = Guid.NewGuid(), Description = "Task 1", AssignedDate = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(1), Status = "Open", AssignedBy = Guid.NewGuid(), EmployeeId = Guid.NewGuid(), FeatureId = Guid.NewGuid() },
                new TaskEntity { TaskId = Guid.NewGuid(), Description = "Task 2", AssignedDate = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(2), Status = "Closed", AssignedBy = Guid.NewGuid(), EmployeeId = Guid.NewGuid(), FeatureId = Guid.NewGuid() }
            };
            await dbContext.Tasks.AddRangeAsync(tasks);
            await dbContext.SaveChangesAsync();

            var repo = new TaskRepository(dbContext);

            // Act
            var result = await repo.GetAllTasks();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, t => t.Description == "Task 1");
            Assert.Contains(result, t => t.Description == "Task 2");
        }

        [Fact]
        public async Task GetTaskByEmployeeIdAsync_Should_Filter_By_EmployeeId()
        {
            // Arrange
            var dbContext = GetDbContext();
            var employeeId = Guid.NewGuid();
            var otherId = Guid.NewGuid();

            var tasks = new[]
            {
                new TaskEntity { TaskId = Guid.NewGuid(), Description = "Employee Task", AssignedDate = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(1), Status = "Open", AssignedBy = Guid.NewGuid(), EmployeeId = employeeId, FeatureId = Guid.NewGuid() },
                new TaskEntity { TaskId = Guid.NewGuid(), Description = "Other Task",    AssignedDate = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(1), Status = "Open", AssignedBy = Guid.NewGuid(), EmployeeId = otherId,   FeatureId = Guid.NewGuid() }
            };
            await dbContext.Tasks.AddRangeAsync(tasks);
            await dbContext.SaveChangesAsync();

            var repo = new TaskRepository(dbContext);

            // Act
            var result = await repo.GetTaskByEmployeeIdAsync(employeeId);

            // Assert
            Assert.Single(result);
            Assert.All(result, t => Assert.Equal(employeeId, t.EmployeeId));
        }

        [Fact]
        public async Task AddTaskAsync_Should_Add_And_Return_Task()
        {
            // Arrange
            var dbContext = GetDbContext();
            var repo = new TaskRepository(dbContext);

            var newTask = new TaskEntity
            {
                Description = "New Task",
                AssignedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(3),
                Status = "Open",
                AssignedBy = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                FeatureId = Guid.NewGuid()
            };

            // Act
            var created = await repo.AddTaskAsync(newTask);

            // Assert
            Assert.NotEqual(Guid.Empty, created.TaskId);
            Assert.Equal("New Task", created.Description);
            Assert.Single(dbContext.Tasks);
            var stored = await dbContext.Tasks.FirstAsync();
            Assert.Equal(created.TaskId, stored.TaskId);
        }

        [Fact]
        public async Task UpdateTask_Should_Modify_And_Return_Task_When_Found()
        {
            // Arrange
            var dbContext = GetDbContext();
            var original = new TaskEntity
            {
                TaskId = Guid.NewGuid(),
                Description = "Old Desc",
                AssignedDate = DateTime.UtcNow.AddDays(-2),
                DueDate = DateTime.UtcNow.AddDays(1),
                Status = "Open",
                AssignedBy = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                FeatureId = Guid.NewGuid()
            };
            await dbContext.Tasks.AddAsync(original);
            await dbContext.SaveChangesAsync();

            var repo = new TaskRepository(dbContext);
            var update = new TaskEntity
            {
                Description = "Updated Desc",
                AssignedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(5),
                Status = "In Progress",
                AssignedBy = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                FeatureId = Guid.NewGuid()
            };

            // Act
            var result = await repo.UpdateTask(original.TaskId, update);
            var stored = await dbContext.Tasks.FirstAsync(t => t.TaskId == original.TaskId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Desc", stored.Description);
            Assert.Equal(update.Status, stored.Status);
            Assert.Equal(update.AssignedBy, stored.AssignedBy);
            Assert.Equal(update.EmployeeId, stored.EmployeeId);
            Assert.Equal(update.FeatureId, stored.FeatureId);
        }

        [Fact]
        public async Task UpdateTask_Should_Return_Null_When_Not_Found()
        {
            // Arrange
            var dbContext = GetDbContext();
            var repo = new TaskRepository(dbContext);
            var update = new TaskEntity
            {
                Description = "Won't Update",
                AssignedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(1),
                Status = "Closed",
                AssignedBy = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                FeatureId = Guid.NewGuid()
            };

            // Act
            var result = await repo.UpdateTask(Guid.NewGuid(), update);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteTask_Should_Remove_And_Return_True_When_Found()
        {
            // Arrange
            var dbContext = GetDbContext();
            var task = new TaskEntity
            {
                TaskId = Guid.NewGuid(),
                Description = "To be deleted",
                AssignedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(1),
                Status = "Open",
                AssignedBy = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                FeatureId = Guid.NewGuid()
            };
            await dbContext.Tasks.AddAsync(task);
            await dbContext.SaveChangesAsync();

            var repo = new TaskRepository(dbContext);

            // Act
            var success = await repo.DeleteTask(task.TaskId);
            var exists = await dbContext.Tasks.AnyAsync(t => t.TaskId == task.TaskId);

            // Assert
            Assert.True(success);
            Assert.False(exists);
        }

        [Fact]
        public async Task DeleteTask_Should_Return_False_When_Not_Found()
        {
            // Arrange
            var dbContext = GetDbContext();
            var repo = new TaskRepository(dbContext);

            // Act
            var success = await repo.DeleteTask(Guid.NewGuid());

            // Assert
            Assert.False(success);
        }
    }
}