
using Azure.Core;
using Employee.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Employee.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<EmployeeEntity> Employees { get; set; }
       
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<SalaryEntity> Salaries { get; set; }
        public DbSet<LeaveEntity> Leaves { get; set; }
        public DbSet<RolesEntity> Roles { get; set; }
        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<FeatureEntity> Features { get; set; }
        public DbSet<PerformanceEntity> Performances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Email as unique
            modelBuilder.Entity<EmployeeEntity>()
                .HasIndex(e => e.Email)
                .IsUnique();
            modelBuilder.Entity<LeaveEntity>()
                .Property(r =>r.Status)
                .HasConversion<int>();

        }
        


    }
}
