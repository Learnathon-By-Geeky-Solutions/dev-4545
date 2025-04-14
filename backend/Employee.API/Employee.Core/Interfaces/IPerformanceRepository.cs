using Employee.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Core.Interfaces
{
    public interface IPerformanceRepository
    {
        Task<IEnumerable<PerformanceEntity>> GetAllPerformances();
        Task<PerformanceEntity?> GetPerformancesByEmployeeId(Guid EmployeeId);
        Task<PerformanceEntity> AddPerformanceAsync(PerformanceEntity performance);
        Task<PerformanceEntity?> UpdatePerformance(Guid Id, PerformanceEntity performance);
        Task<bool> DeletePerformance(Guid Id);
    }
}
