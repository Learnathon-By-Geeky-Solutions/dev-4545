using Employee.Core.Entities;
using Employee.Core.Enums;
using Employee.Infrastructure.Data;
using Employee.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeXUnit.Test.InfrastructureLayer
{
    public class LeaveRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly LeaveRepository _repo;

        public LeaveRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _repo = new LeaveRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private LeaveEntity CreateSampleLeave(Guid? employeeId = null)
        {
            return new LeaveEntity
            {
                StartDate = new DateOnly(2025, 1, 1),
                EndDate = new DateOnly(2025, 1, 5),
                LeaveType = "Sick",
                Status = Status.Pending,
                Reason = "Flu",
                EmployeeId = employeeId ?? Guid.NewGuid()
            };
        }

        [Fact]
        public async Task AddLeave_ShouldAssignNewIdAndAddLeave()
        {
            // Arrange
            var leave = CreateSampleLeave();

            // Act
            var result = await _repo.AddLeave(leave);

            // Assert
            Assert.NotEqual(Guid.Empty, result.LeaveId);
            Assert.Single(_context.Leaves);
            var added = await _context.Leaves.FirstAsync();
            Assert.Equal(result.LeaveId, added.LeaveId);
        }

        [Fact]
        public async Task DeleteLeaveByEmployeeId_ShouldReturnTrue_IfLeaveExists()
        {
            // Arrange
            var leave = CreateSampleLeave();
            await _context.Leaves.AddAsync(leave);
            await _context.SaveChangesAsync();

            // Act
            var deleted = await _repo.DeleteLeaveByEmployeeId(leave.EmployeeId);

            // Assert
            Assert.True(deleted);
            Assert.Empty(_context.Leaves);
        }

        [Fact]
        public async Task DeleteLeaveByEmployeeId_ShouldReturnFalse_IfLeaveNotExists()
        {
            // Act
            var deleted = await _repo.DeleteLeaveByEmployeeId(Guid.NewGuid());

            // Assert
            Assert.False(deleted);
        }

        [Fact]
        public async Task GetLeaveByEmployeeId_ShouldReturnLeave_IfExists()
        {
            // Arrange
            var leave = CreateSampleLeave();
            await _context.Leaves.AddAsync(leave);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetLeaveByEmployeeId(leave.EmployeeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(leave.EmployeeId, result!.EmployeeId);
            Assert.Equal(leave.LeaveType, result.LeaveType);
        }

        [Fact]
        public async Task GetLeaveByEmployeeId_ShouldReturnNull_IfNotExists()
        {
            // Act
            var result = await _repo.GetLeaveByEmployeeId(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetLeaves_ShouldReturnAllLeaves()
        {
            // Arrange
            var l1 = CreateSampleLeave();
            var l2 = CreateSampleLeave();
            await _context.Leaves.AddRangeAsync(l1, l2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetLeaves();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, x => x.LeaveId == l1.LeaveId);
            Assert.Contains(result, x => x.LeaveId == l2.LeaveId);
        }

        [Fact]
        public async Task UpdateLeave_ShouldUpdateAndReturnLeave_IfExists()
        {
            // Arrange
            var leave = CreateSampleLeave();
            await _context.Leaves.AddAsync(leave);
            await _context.SaveChangesAsync();

            var updated = new LeaveEntity
            {
                StartDate = new DateOnly(2025, 2, 1),
                EndDate = new DateOnly(2025, 2, 3),
                LeaveType = "Vacation",
                Status = Status.Approved,
                Reason = "Family trip",
                EmployeeId = leave.EmployeeId
            };

            // Act
            var result = await _repo.UpdateLeave(leave.EmployeeId, updated);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updated.StartDate, result!.StartDate);
            Assert.Equal(updated.EndDate, result.EndDate);
            Assert.Equal("Vacation", result.LeaveType);
            Assert.Equal(Status.Approved, result.Status);
            Assert.Equal("Family trip", result.Reason);
        }

        [Fact]
        public async Task UpdateLeave_ShouldReturnNull_IfLeaveNotExists()
        {
            // Arrange
            var updated = CreateSampleLeave();

            // Act
            var result = await _repo.UpdateLeave(Guid.NewGuid(), updated);

            // Assert
            Assert.Null(result);
        }
    }
}
