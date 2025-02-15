
using Employee.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Employee.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
    }
}
