using Employee.Core.Entities;

namespace Employee.Core.Interfaces
{
    public interface IRolesRepository
    {
        Task<IEnumerable<RolesEntity>> GetRoles();
        Task<RolesEntity> GetRolesById(Guid RolesId);
        Task<RolesEntity> AddRoles(RolesEntity Roles);
        Task<RolesEntity> UpdateRoles(Guid RolesId, RolesEntity updateRoles);
        Task<bool> DeleteRolesById(Guid EmployeeId);
    }
}
