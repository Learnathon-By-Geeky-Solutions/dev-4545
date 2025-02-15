using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Employee.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Employee.Infrastructure.Repositories
{
    public class EmployeeRepository(AppDbContext dbContext) : IEmployeeRepository
    {
        public async Task<IEnumerable<EmployeeEntity>> GetEmployees()
        {
            return await dbContext.Employees.ToListAsync();

        }
        public async Task<EmployeeEntity> GetEmployeeById(Guid id)
        {
            return await dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);
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
