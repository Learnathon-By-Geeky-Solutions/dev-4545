using Employee.Core.Entities;
using Employee.Infrastructure.Data;
using Employee.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeXUnit.Test.InfrastructureLayer
{
    public class FeatureRepositoryTests
    {
        private DbContextOptions<AppDbContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task AddFeatureAsync_AddsFeatureAndReturnsIt()
        {
            // Arrange
            using var context = new AppDbContext(GetInMemoryOptions());
            var repository = new FeatureRepository(context);
            var feature = new FeatureEntity
            {
                ProjectId = Guid.NewGuid(),
                FeatureName = "New Feature",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                Description = "Description"
            };

            // Act
            var result = await repository.AddFeatureAsync(feature);

            // Assert
            Assert.NotEqual(Guid.Empty, result.FeatureId);
            Assert.Equal(feature.ProjectId, result.ProjectId);
            Assert.Equal(feature.FeatureName, result.FeatureName);
            Assert.Equal(feature.StartDate, result.StartDate);
            Assert.Equal(feature.EndDate, result.EndDate);
            Assert.Equal(feature.Description, result.Description);

            var savedFeature = await context.Features.FindAsync(result.FeatureId);
            Assert.NotNull(savedFeature);
            Assert.Equal(result.FeatureId, savedFeature.FeatureId);
        }

        [Fact]
        public async Task DeleteFeature_ExistingFeatureWithTasks_DeletesFeatureAndTasks()
        {
            // Arrange
            using var context = new AppDbContext(GetInMemoryOptions());
            var feature = new FeatureEntity { FeatureId = Guid.NewGuid(), ProjectId = Guid.NewGuid(), FeatureName = "Feature" };
            var task1 = new TaskEntity { TaskId = Guid.NewGuid(), FeatureId = feature.FeatureId, EmployeeId = Guid.NewGuid() };
            var task2 = new TaskEntity { TaskId = Guid.NewGuid(), FeatureId = feature.FeatureId, EmployeeId = Guid.NewGuid() };
            await context.Features.AddAsync(feature);
            await context.Tasks.AddRangeAsync(task1, task2);
            await context.SaveChangesAsync();

            var repository = new FeatureRepository(context);

            // Act
            var result = await repository.DeleteFeature(feature.FeatureId);

            // Assert
            Assert.True(result);
            var deletedFeature = await context.Features.FindAsync(feature.FeatureId);
            Assert.Null(deletedFeature);
            var remainingTasks = await context.Tasks.Where(t => t.FeatureId == feature.FeatureId).ToListAsync();
            Assert.Empty(remainingTasks);
        }

        [Fact]
        public async Task DeleteFeature_ExistingFeatureNoTasks_DeletesFeature()
        {
            // Arrange
            using var context = new AppDbContext(GetInMemoryOptions());
            var feature = new FeatureEntity { FeatureId = Guid.NewGuid(), ProjectId = Guid.NewGuid(), FeatureName = "Feature" };
            await context.Features.AddAsync(feature);
            await context.SaveChangesAsync();

            var repository = new FeatureRepository(context);

            // Act
            var result = await repository.DeleteFeature(feature.FeatureId);

            // Assert
            Assert.True(result);
            var deletedFeature = await context.Features.FindAsync(feature.FeatureId);
            Assert.Null(deletedFeature);
        }

        [Fact]
        public async Task DeleteFeature_NonExistingFeature_ReturnsFalse()
        {
            // Arrange
            using var context = new AppDbContext(GetInMemoryOptions());
            var repository = new FeatureRepository(context);
            var nonExistingId = Guid.NewGuid();

            // Act
            var result = await repository.DeleteFeature(nonExistingId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetAllFeatures_ReturnsAllFeatures()
        {
            // Arrange
            using var context = new AppDbContext(GetInMemoryOptions());
            var feature1 = new FeatureEntity { FeatureId = Guid.NewGuid(), ProjectId = Guid.NewGuid(), FeatureName = "Feature1" };
            var feature2 = new FeatureEntity { FeatureId = Guid.NewGuid(), ProjectId = Guid.NewGuid(), FeatureName = "Feature2" };
            await context.Features.AddRangeAsync(feature1, feature2);
            await context.SaveChangesAsync();

            var repository = new FeatureRepository(context);

            // Act
            var result = await repository.GetAllFeatures();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetFeatureByEmployeeId_ReturnsFeaturesForEmployee()
        {
            // Arrange
            using var context = new AppDbContext(GetInMemoryOptions());
            var employeeId = Guid.NewGuid();
            var feature1 = new FeatureEntity { FeatureId = Guid.NewGuid(), ProjectId = Guid.NewGuid(), FeatureName = "Feature1" };
            var feature2 = new FeatureEntity { FeatureId = Guid.NewGuid(), ProjectId = Guid.NewGuid(), FeatureName = "Feature2" };
            var task1 = new TaskEntity { TaskId = Guid.NewGuid(), FeatureId = feature1.FeatureId, EmployeeId = employeeId };
            var task2 = new TaskEntity { TaskId = Guid.NewGuid(), FeatureId = feature2.FeatureId, EmployeeId = employeeId };

            await context.Features.AddRangeAsync(feature1, feature2);
            await context.Tasks.AddRangeAsync(task1, task2);
            await context.SaveChangesAsync();

            var repository = new FeatureRepository(context);

            // Act
            var result = await repository.GetFeatureByEmployeeId(employeeId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, f => f.FeatureId == feature1.FeatureId);
            Assert.Contains(result, f => f.FeatureId == feature2.FeatureId);
        }

        [Fact]
        public async Task GetFeatureByEmployeeId_NoTasks_ReturnsEmpty()
        {
            // Arrange
            using var context = new AppDbContext(GetInMemoryOptions());
            var employeeId = Guid.NewGuid();
            var repository = new FeatureRepository(context);

            // Act
            var result = await repository.GetFeatureByEmployeeId(employeeId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetFeatureByEmployeeId_MultipleTasksSameFeature_ReturnsFeatureOnce()
        {
            // Arrange
            using var context = new AppDbContext(GetInMemoryOptions());
            var employeeId = Guid.NewGuid();
            var feature = new FeatureEntity { FeatureId = Guid.NewGuid(), ProjectId = Guid.NewGuid(), FeatureName = "Feature" };
            var task1 = new TaskEntity { TaskId = Guid.NewGuid(), FeatureId = feature.FeatureId, EmployeeId = employeeId };
            var task2 = new TaskEntity { TaskId = Guid.NewGuid(), FeatureId = feature.FeatureId, EmployeeId = employeeId };

            await context.Features.AddAsync(feature);
            await context.Tasks.AddRangeAsync(task1, task2);
            await context.SaveChangesAsync();

            var repository = new FeatureRepository(context);

            // Act
            var result = await repository.GetFeatureByEmployeeId(employeeId);

            // Assert
            Assert.Single(result);
            Assert.Equal(feature.FeatureId, result.First().FeatureId);
        }

        [Fact]
        public async Task UpdateFeature_ExistingFeature_UpdatesAndReturnsUpdatedFeature()
        {
            // Arrange
            using var context = new AppDbContext(GetInMemoryOptions());
            var feature = new FeatureEntity { FeatureId = Guid.NewGuid(), ProjectId = Guid.NewGuid(), FeatureName = "Old Name" };
            await context.Features.AddAsync(feature);
            await context.SaveChangesAsync();

            var repository = new FeatureRepository(context);
            var updatedFeature = new FeatureEntity { FeatureName = "New Name", ProjectId = Guid.NewGuid() };

            // Act
            var result = await repository.UpdateFeature(feature.FeatureId, updatedFeature);

            // Assert
            Assert.Equal("New Name", result.FeatureName);
            var savedFeature = await context.Features.FindAsync(feature.FeatureId);
            Assert.Equal("New Name", savedFeature.FeatureName);
        }

        [Fact]
        public async Task UpdateFeature_NonExistingFeature_ReturnsInputFeature()
        {
            // Arrange
            using var context = new AppDbContext(GetInMemoryOptions());
            var repository = new FeatureRepository(context);
            var nonExistingId = Guid.NewGuid();
            var feature = new FeatureEntity { FeatureId = Guid.NewGuid(), FeatureName = "Some Name" };

            // Act
            var result = await repository.UpdateFeature(nonExistingId, feature);

            // Assert
            Assert.Equal(feature, result);
            Assert.Equal(feature.FeatureId, result.FeatureId);
            var featuresInDb = await context.Features.ToListAsync();
            Assert.Empty(featuresInDb);
        }
    }
}