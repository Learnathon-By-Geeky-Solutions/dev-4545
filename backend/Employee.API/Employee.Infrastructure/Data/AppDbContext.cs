
using Employee.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Employee.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<EmployeeEntity> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Email as unique
            modelBuilder.Entity<EmployeeEntity>()
                .HasIndex(e => e.Email)
                .IsUnique();
        }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<SalaryEntity> Salaries { get; set; }

    }
}
