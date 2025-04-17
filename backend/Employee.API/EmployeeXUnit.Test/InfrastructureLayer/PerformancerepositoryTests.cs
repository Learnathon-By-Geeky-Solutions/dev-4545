using System;
using System.Linq;
using System.Threading.Tasks;
using Employee.Core.Entities;
using Employee.Infrastructure.Data;
using Employee.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeeXUnit.Test.InfrastructureLayer
{
    public class PerformanceRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly PerformanceRepository _repo;

        public PerformanceRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _repo = new PerformanceRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private PerformanceEntity CreateSamplePerformance(Guid? employeeId = null, Guid? reviewerId = null)
        {
            return new PerformanceEntity
            {
                Date = DateTime.UtcNow.Date,
                Rating = "Excellent",
                Comments = "Great work",
                EmployeeId = employeeId ?? Guid.NewGuid(),
                ReviewerId = reviewerId ?? Guid.NewGuid()
            };
        }

        [Fact]
        public async Task AddPerformanceAsync_ShouldAssignNewIdAndAddPerformance()
        {
            // Arrange
            var perf = CreateSamplePerformance();

            // Act
            var result = await _repo.AddPerformanceAsync(perf);

            // Assert
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Single(_context.Performances);
            var stored = await _context.Performances.FirstAsync();
            Assert.Equal(result.Id, stored.Id);
        }

        [Fact]
        public async Task DeletePerformance_ShouldReturnTrue_IfExists()
        {
            // Arrange
            var perf = CreateSamplePerformance();
            await _context.Performances.AddAsync(perf);
            await _context.SaveChangesAsync();

            // Act
            var deleted = await _repo.DeletePerformance(perf.Id);

            // Assert
            Assert.True(deleted);
            Assert.Empty(_context.Performances);
        }

        [Fact]
        public async Task DeletePerformance_ShouldReturnFalse_IfNotExists()
        {
            // Act
            var deleted = await _repo.DeletePerformance(Guid.NewGuid());

            // Assert
            Assert.False(deleted);
        }

        [Fact]
        public async Task GetAllPerformances_ShouldReturnAll()
        {
            // Arrange
            var p1 = CreateSamplePerformance();
            var p2 = CreateSamplePerformance();
            await _context.Performances.AddRangeAsync(p1, p2);
            await _context.SaveChangesAsync();

            // Act
            var list = await _repo.GetAllPerformances();

            // Assert
            Assert.Equal(2, list.Count());
            Assert.Contains(list, x => x.Id == p1.Id);
            Assert.Contains(list, x => x.Id == p2.Id);
        }

        [Fact]
        public async Task GetPerformancesByEmployeeId_ShouldReturnPerformance_IfExists()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var perf = CreateSamplePerformance(employeeId: employeeId);
            await _context.Performances.AddAsync(perf);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetPerformancesByEmployeeId(employeeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(employeeId, result!.EmployeeId);
            Assert.Equal(perf.Rating, result.Rating);
        }

        [Fact]
        public async Task GetPerformancesByEmployeeId_ShouldReturnNull_IfNotExists()
        {
            // Act
            var result = await _repo.GetPerformancesByEmployeeId(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdatePerformance_ShouldUpdateAndReturn_IfExists()
        {
            // Arrange
            var perf = CreateSamplePerformance();
            await _context.Performances.AddAsync(perf);
            await _context.SaveChangesAsync();

            var updated = new PerformanceEntity
            {
                Date = perf.Date.AddDays(1),
                Rating = "Good",
                Comments = "Improved",
                EmployeeId = perf.EmployeeId,
                ReviewerId = perf.ReviewerId
            };

            // Act
            var result = await _repo.UpdatePerformance(perf.Id, updated);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updated.Date, result!.Date);
            Assert.Equal("Good", result.Rating);
            Assert.Equal("Improved", result.Comments);
        }

        [Fact]
        public async Task UpdatePerformance_ShouldReturnNull_IfNotExists()
        {
            // Arrange
            var updated = CreateSamplePerformance();

            // Act
            var result = await _repo.UpdatePerformance(Guid.NewGuid(), updated);

            // Assert
            Assert.Null(result);
        }
    }
}
