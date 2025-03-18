using Employee.Core.Entities;

namespace Employee.Core.Interfaces
{
    public interface ISalaryRepository
    {
        Task<IEnumerable<SalaryEntity>> GetSalaries();
        Task<SalaryEntity> GetSalaryByEmployeeId(Guid EmployeeId);
        Task<SalaryEntity> AddSalary(SalaryEntity salary);
        Task<SalaryEntity> UpdateSalary(Guid EmployeeId, SalaryEntity updateSalary);
        Task<bool> DeleteSalaryByEmployeeId(Guid EmployeeId);
    }
}
