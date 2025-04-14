using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Employee.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Employee.Infrastructure.Repositories
{
    public class LeaveRepository(AppDbContext dbContext) : ILeaveRepository
    {
        public async Task<LeaveEntity> AddLeave(LeaveEntity Leave)
        {
            Leave.LeaveId = Guid.NewGuid();
            await dbContext.Leaves.AddAsync(Leave);
            await dbContext.SaveChangesAsync();
            return Leave;
        }

        public async Task<bool> DeleteLeaveByEmployeeId(Guid EmployeeId)
        {
            var result = await dbContext.Leaves.FirstOrDefaultAsync(x=>x.EmployeeId == EmployeeId);
            if(result == null)
            {
                return false;
            }
            else
            {
                dbContext.Leaves.Remove(result);
                return await dbContext.SaveChangesAsync() >0;
            }
        }

        public async Task<LeaveEntity?> GetLeaveByEmployeeId(Guid EmployeeId)
        {
            var result = await dbContext.Leaves.FirstOrDefaultAsync(x=>x.EmployeeId==EmployeeId);
            return result;
        }

        public async Task<IEnumerable<LeaveEntity>> GetLeaves()
        {
            var result = await dbContext.Leaves.ToListAsync();
            return result;

        }

        public async Task<LeaveEntity?> UpdateLeave(Guid EmployeeId, LeaveEntity updateLeave)
        {
            var result = await dbContext.Leaves.FirstOrDefaultAsync(x=>x.EmployeeId==EmployeeId);
            if (result==null)
            {
                return null;
            }
            else
            {
                result.StartDate = updateLeave.StartDate;
                result.EndDate= updateLeave.EndDate;
                result.EmployeeId = EmployeeId;
                result.Status = updateLeave.Status;
                result.LeaveType = updateLeave.LeaveType;
                result.Reason = updateLeave.Reason;

                await dbContext.SaveChangesAsync();
                return result;
            }
        }
    }
}
