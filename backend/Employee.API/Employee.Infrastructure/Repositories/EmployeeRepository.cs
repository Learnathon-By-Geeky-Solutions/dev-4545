using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Employee.Infrastructure.Data;
using Employee.Infrastructure.Services;
using Management.Core.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Employee.Infrastructure.Repositories
{
    public class EmployeeRepository(AppDbContext dbContext, IConfiguration _configuration) : IEmployeeRepository
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
            var salt = PasswordHasher.GenerateSalt();
            var hashedPassword = PasswordHasher.HashPassword(employee.Password, salt);
            employee.EmployeeId= Guid.NewGuid();
            employee.Salt = salt;
            employee.Password = hashedPassword;
            await dbContext.Employees.AddAsync(employee);  
            dbContext.SaveChanges();
            return employee;
        }

        public async Task<EmployeeEntity> UpdateEmployee(Guid id, EmployeeEntity updatedentity)
        {
            var salt = PasswordHasher.GenerateSalt();
            var hashedPassword = PasswordHasher.HashPassword(updatedentity.Password, salt);
            var data = await dbContext.Employees.FirstOrDefaultAsync(x=>x.EmployeeId == id);
            if (data!=null)
            {
                data.Name = updatedentity.Name;
                data.Email = updatedentity.Email;
                data.Phone = updatedentity.Phone;
                data.DateOfJoin = updatedentity.DateOfJoin;
                data.Password = hashedPassword;
                data.Salt = salt;
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
        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request)
        {
            var user = await dbContext.Employees.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (user == null)
            {
                throw new ApplicationException($"user is not found with this Email : {request.Email}");
            }

            var password = PasswordHasher.HashPassword(request.Password, user.Salt);
            var succeed = await dbContext.Employees.FirstOrDefaultAsync(x => x.Password == password);
            
            if (succeed == null)
            {
                throw new ApplicationException($"Password isn't correct");

            }
            var accessTokenService = new AccessTokenService(_configuration);
            var JwtSecurity = await accessTokenService.GenerateToken(user);

            var authenticationResponse = new AuthenticationResponse();
            var refreshTokenEntity = new RefreshTokenEntity();

            var RefreshToken = RefreshTokenService.GenerateRefreshToken();

            authenticationResponse.Role = "Admin";
            authenticationResponse.RefreshToken = RefreshToken;
            authenticationResponse.JwToken = new JwtSecurityTokenHandler().WriteToken(JwtSecurity);

            refreshTokenEntity.TokenId = Guid.NewGuid();
            refreshTokenEntity.EmployeeId = user.EmployeeId;
            refreshTokenEntity.RefreshToken = RefreshToken;
            await dbContext.RefreshTokens.AddAsync(refreshTokenEntity);
            dbContext.SaveChanges();

            return authenticationResponse;
        }

       



    }
}
