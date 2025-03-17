using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Employee.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Employee.Infrastructure.Repositories
{
    public class SalaryRepository(AppDbContext dbContext) : ISalaryRepository
    {
        public async Task<SalaryEntity> AddSalary(SalaryEntity salary)
        {
            await dbContext.Salaries.AddAsync(salary);
            dbContext.SaveChanges();
            return salary;

        }

        public async Task<bool> DeleteSalaryByEmployeeId(Guid EmployeeId)
        {
            var result = await dbContext.Salaries.FirstOrDefaultAsync(x=> x.EmployeeId==EmployeeId);
            if (result != null) {
                dbContext.Salaries.Remove(result);
                return await dbContext.SaveChangesAsync() > 0;
            }
            return false;


        }

        public async Task<IEnumerable<SalaryEntity>> GetSalaries()
        {
            var result = await dbContext.Salaries.ToListAsync();
            return result;
        }

        public async Task<SalaryEntity> GetSalaryByEmployeeId(Guid EmployeeId)
        {
            var result = await dbContext.Salaries.FirstOrDefaultAsync(x=>x.EmployeeId==EmployeeId);
            return result;
        }

        public async Task<SalaryEntity> UpdateSalary(Guid EmployeeId, SalaryEntity updateSalary)
        {
            var result = dbContext.Salaries.FirstOrDefault(x => x.EmployeeId == EmployeeId);
            if (result != null) {
                result.EmployeeId = updateSalary.EmployeeId;
                result.SalaryDate = updateSalary.SalaryDate;
                result.Amount = updateSalary.Amount;
                await dbContext.SaveChangesAsync();
            }
            return result;
            
        }
    }
}
