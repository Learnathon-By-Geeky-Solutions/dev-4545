using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Employee.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Employee.Infrastructure.Repositories
{
    public class EmployeeRepository(AppDbContext dbContext, IDistributedCache memorycache) : IEmployeeRepository
    {
        private readonly IDistributedCache distributedCache = memorycache;
        
        public async Task<IEnumerable<EmployeeEntity>> GetEmployees()
        {
            return await dbContext.Employees.ToListAsync();

        }
        public async Task<EmployeeEntity?> GetEmployeeById(Guid id)
        {
            string key= $"employee-{id}";
            string? cachedemployee =await distributedCache.GetStringAsync(
                key
                );
            EmployeeEntity? result;
            if (cachedemployee == null) {
                result= await dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);
                if (result == null) {
                    return null;
                }
                await distributedCache.SetStringAsync(key,
                    JsonConvert.SerializeObject(result)
                 );
                return result;
            }
            result = JsonConvert.DeserializeObject<EmployeeEntity>(cachedemployee);


            return result;
        }
        public async Task<EmployeeEntity> AddEmployee(EmployeeEntity employee)
        {
            employee.EmployeeId= Guid.NewGuid();
            await dbContext.Employees.AddAsync(employee);  
            dbContext.SaveChanges();
            return employee;
        }

        public async Task<EmployeeEntity> UpdateEmployee(Guid id, EmployeeEntity updatedentity)
        {
            var data = await dbContext.Employees.FirstOrDefaultAsync(x=>x.EmployeeId == id);
            if (data!=null)
            {
                data.Name = updatedentity.Name;
                data.Email = updatedentity.Email;
                data.Phone = updatedentity.Phone;
                data.DateOfJoin = updatedentity.DateOfJoin;
                data.Password = updatedentity.Password;
                data.Phone= updatedentity.Phone;
                data.Stack = updatedentity.Stack;


                await dbContext.SaveChangesAsync();
                return data;
            }
            return updatedentity;
        }

        public async Task<bool> DeleteEmployee(Guid id)
        {
            var data =await dbContext.Employees.FirstOrDefaultAsync(x=>x.EmployeeId==id);
            if (data != null)
            {
                dbContext.Employees.Remove(data);
                return await dbContext.SaveChangesAsync() > 0;
            }
            return false; 
        }



    }
}
