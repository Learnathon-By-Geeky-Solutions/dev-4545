using Employee.Core.Entities;
using Employee.Infrastructure.Data;
using Employee.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeXUnit.Test.InfrastructureLayer
{
    public class SalaryRepositoryTests
    {
        private static AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddSalary_Should_Add_And_Return_Entity_With_Id()
        {
            // Arrange
            var dbContext = GetDbContext();
            var repo = new SalaryRepository(dbContext);
            var salary = new SalaryEntity
            {
                Amount = 5000f,
                SalaryDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EmployeeId = Guid.NewGuid()
            };

            // Act
            var result = await repo.AddSalary(salary);

            // Assert
            Assert.NotEqual(Guid.Empty, result.SalaryId);
            Assert.Equal(5000f, result.Amount);
            Assert.Single(dbContext.Salaries);
            var stored = await dbContext.Salaries.FirstAsync();
            Assert.Equal(result.SalaryId, stored.SalaryId);
        }

        [Fact]
        public async Task GetSalaries_Should_Return_All_Salaries()
        {
            // Arrange
            var dbContext = GetDbContext();
            var salaries = new[]
            {
                new SalaryEntity { SalaryId = Guid.NewGuid(), Amount = 1000f, SalaryDate = DateOnly.FromDateTime(DateTime.UtcNow), EmployeeId = Guid.NewGuid() },
                new SalaryEntity { SalaryId = Guid.NewGuid(), Amount = 2000f, SalaryDate = DateOnly.FromDateTime(DateTime.UtcNow), EmployeeId = Guid.NewGuid() }
            };
            await dbContext.Salaries.AddRangeAsync(salaries);
            await dbContext.SaveChangesAsync();

            var repo = new SalaryRepository(dbContext);

            // Act
            var all = await repo.GetSalaries();

            // Assert
            Assert.Equal(2, all.Count());
            Assert.Contains(all, s => s.Amount == 1000f);
            Assert.Contains(all, s => s.Amount == 2000f);
        }

        [Fact]
        public async Task GetSalaryByEmployeeId_Should_Return_Correct_Entity_Or_Null()
        {
            // Arrange
            var dbContext = GetDbContext();
            var empId = Guid.NewGuid();
            dbContext.Salaries.Add(new SalaryEntity
            {
                SalaryId = Guid.NewGuid(),
                Amount = 1500f,
                SalaryDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EmployeeId = empId
            });
            await dbContext.SaveChangesAsync();

            var repo = new SalaryRepository(dbContext);

            // Act
            var found = await repo.GetSalaryByEmployeeId(empId);
            var notFound = await repo.GetSalaryByEmployeeId(Guid.NewGuid());

            // Assert
            Assert.NotNull(found);
            Assert.Equal(1500f, found!.Amount);
            Assert.Null(notFound);
        }

        [Fact]
        public async Task DeleteSalaryByEmployeeId_Should_Remove_And_Return_True_When_Found()
        {
            // Arrange
            var dbContext = GetDbContext();
            var empId = Guid.NewGuid();
            dbContext.Salaries.Add(new SalaryEntity
            {
                SalaryId = Guid.NewGuid(),
                Amount = 3000f,
                SalaryDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EmployeeId = empId
            });
            await dbContext.SaveChangesAsync();

            var repo = new SalaryRepository(dbContext);

            // Act
            var deleted = await repo.DeleteSalaryByEmployeeId(empId);
            var stillThere = await dbContext.Salaries.AnyAsync(s => s.EmployeeId == empId);

            // Assert
            Assert.True(deleted);
            Assert.False(stillThere);
        }

        [Fact]
        public async Task DeleteSalaryByEmployeeId_Should_Return_False_When_Not_Found()
        {
            // Arrange
            var dbContext = GetDbContext();
            var repo = new SalaryRepository(dbContext);

            // Act
            var deleted = await repo.DeleteSalaryByEmployeeId(Guid.NewGuid());

            // Assert
            Assert.False(deleted);
        }

        [Fact]
        public async Task UpdateSalary_Should_Modify_And_Return_Entity_When_Found()
        {
            // Arrange
            var dbContext = GetDbContext();
            var empId = Guid.NewGuid();
            var original = new SalaryEntity
            {
                SalaryId = Guid.NewGuid(),
                Amount = 2500f,
                SalaryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
                EmployeeId = empId
            };
            await dbContext.Salaries.AddAsync(original);
            await dbContext.SaveChangesAsync();

            var repo = new SalaryRepository(dbContext);
            var updatedInfo = new SalaryEntity
            {
                EmployeeId = empId,
                Amount = 3500f,
                SalaryDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            // Act
            var updated = await repo.UpdateSalary(empId, updatedInfo);
            var stored = await dbContext.Salaries.FirstAsync(s => s.EmployeeId == empId);

            // Assert
            Assert.NotNull(updated);
            Assert.Equal(3500f, stored.Amount);
            Assert.Equal(updatedInfo.SalaryDate, stored.SalaryDate);
        }

        [Fact]
        public async Task UpdateSalary_Should_Return_Null_When_Not_Found()
        {
            // Arrange
            var dbContext = GetDbContext();
            var repo = new SalaryRepository(dbContext);
            var updatedInfo = new SalaryEntity
            {
                EmployeeId = Guid.NewGuid(),
                Amount = 4000f,
                SalaryDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            // Act
            var updated = await repo.UpdateSalary(Guid.NewGuid(), updatedInfo);

            // Assert
            Assert.Null(updated);
        }
    }
}
