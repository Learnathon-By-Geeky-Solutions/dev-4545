using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Employee.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Employee.Infrastructure.Repositories
{
    public class RolesRepository(AppDbContext dbContext) : IRolesRepository
    {
        public async Task<RolesEntity> AddRoles(RolesEntity Role)
        {
            Role.RoleId = new Guid();
            await dbContext.Roles.AddAsync(Role);
            await dbContext.SaveChangesAsync();

            return Role;
        }

        public async Task<bool> DeleteRolesById(Guid RolesId)
        {
            var result = await dbContext.Roles.FirstOrDefaultAsync(x => x.RoleId==RolesId);
            if (result != null)
            {
                dbContext.Roles.Remove(result);
                return await dbContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<RolesEntity>> GetRoles()
        {
            IEnumerable<RolesEntity> result = await dbContext.Roles.ToListAsync();
            return result;
        }

        public async Task<RolesEntity> GetRolesById(Guid RoleId)
        {
            var result = await dbContext.Roles.FirstOrDefaultAsync(x => x.RoleId==RoleId);
            return result;
        }

        public  async Task<RolesEntity> UpdateRoles(Guid RoleId, RolesEntity updateRoles)
        {
            var result =await dbContext.Roles.FirstOrDefaultAsync(x => x.RoleId == RoleId);
            if (result != null)
            {
                result.Permissions = updateRoles.Permissions;
                result.Descriptions = updateRoles.Descriptions; 
                result.RoleName = updateRoles.RoleName; 
                await dbContext.SaveChangesAsync();
            }
            return result;

        }
    }
}
