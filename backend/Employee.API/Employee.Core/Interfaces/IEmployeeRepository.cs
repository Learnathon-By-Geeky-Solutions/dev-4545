
using Employee.Core.Entities;
using Management.Core.DTO;

namespace Employee.Core.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeEntity>> GetEmployees();
        Task<EmployeeEntity> GetEmployeeById(Guid id);
        Task<EmployeeEntity> AddEmployee(EmployeeEntity employee);
        Task<EmployeeEntity?> UpdateEmployee(Guid id, EmployeeEntity updatedentity);
        Task<bool> DeleteEmployee(Guid id);
        Task<AuthenticationResponse> Authenticate(AuthenticationRequest request);
    }
}
