using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Core.Entities;

namespace Employee.Core.Interfaces
{
    public interface ILeaveRepository
    {
        Task<IEnumerable<LeaveEntity>> GetLeaves();
        Task<LeaveEntity?> GetLeaveByEmployeeId(Guid EmployeeId);
        Task<LeaveEntity> AddLeave(LeaveEntity Leave);
        Task<LeaveEntity?> UpdateLeave(Guid EmployeeId, LeaveEntity updateLeave);
        Task<bool> DeleteLeaveByEmployeeId(Guid EmployeeId);
    }
}
