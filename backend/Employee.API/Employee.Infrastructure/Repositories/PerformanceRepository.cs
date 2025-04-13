using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Employee.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Employee.Infrastructure.Repositories
{
    public class PerformanceRepository(AppDbContext dbContext) : IPerformanceRepository
    {
        public async Task<PerformanceEntity> AddPerformanceAsync(PerformanceEntity performance)
        {
            performance.Id = Guid.NewGuid();
            await dbContext.Performances.AddAsync(performance);
            await dbContext.SaveChangesAsync();
            return performance ;
        }

        public async Task<bool> DeletePerformance(Guid Id)
        {
            var performance = await dbContext.Performances.FirstOrDefaultAsync(x => x.Id == Id);
            if (performance != null)
            {
                dbContext.Performances.Remove(performance);
                return await dbContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<PerformanceEntity>> GetAllPerformances()
        {
            var performance = await dbContext.Performances.ToListAsync();
            return performance;
        }

        public async Task<PerformanceEntity> GetPerformancesByEmployeeId(Guid EmployeeId)
        {
            var performance = await dbContext.Performances.FirstOrDefaultAsync(x => x.EmployeeId == EmployeeId);
            return performance;
        }

        public async Task<PerformanceEntity> UpdatePerformance(Guid Id, PerformanceEntity performance)
        {
            var data = await dbContext.Performances.FirstOrDefaultAsync(x => x.Id == Id);
            if (data != null)
            {
                data.Date = performance.Date;
                data.Rating = performance.Rating;
                data.Comments = performance.Comments;
                data.EmployeeId = performance.EmployeeId;
                data.ReviewerId = performance.ReviewerId;
                
                await dbContext.SaveChangesAsync();
                return data;

            }
            else
            {
                return null;
            }
                
        }
    }
}
