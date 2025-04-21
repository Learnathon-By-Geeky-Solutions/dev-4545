using System.IdentityModel.Tokens.Jwt;
using Employee.Application.Common.Interfaces;
using Employee.Application.Interfaces;
using Employee.Core.Entities;
using Employee.Core.Enums;
using Employee.Core.Interfaces;
using Employee.Infrastructure.Data;
using Employee.Infrastructure.Services;
using Management.Core.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Employee.Infrastructure.Repositories
{
    public class EmployeeRepository(AppDbContext dbContext, ICacheService cacheService, IConfiguration configuration, IAccessTokenService accessTokenService) : IEmployeeRepository
    {
        private const int CacheExpirationInSeconds = 30;

        private readonly AppDbContext _dbContext = dbContext;
        private readonly ICacheService _cacheService = cacheService;
        private readonly IConfiguration _configuration = configuration;
        private readonly IAccessTokenService? _accessTokenService = accessTokenService;

        public async Task<IEnumerable<EmployeeEntity>> GetEmployees()
        {
            return await _dbContext.Employees.ToListAsync();
        }

        public async Task<EmployeeEntity?> GetEmployeeById(Guid id)
        {
            string cacheKey = $"employee-{id}";

            var cachedEmployee = await _cacheService.GetAsync<EmployeeEntity>(cacheKey);
            if (cachedEmployee != null)
            {
                return cachedEmployee;
            }

            var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (employee != null)
            {
                await _cacheService.SetAsync(cacheKey, employee, TimeSpan.FromSeconds(CacheExpirationInSeconds));
            }

            return employee;
        }

        public async Task<EmployeeEntity> AddEmployee(EmployeeEntity employee)
        {
            employee.EmployeeId = Guid.NewGuid();
            SetPasswordHash(employee, employee.Password);

            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<EmployeeEntity?> UpdateEmployee(Guid id, EmployeeEntity updatedentity)
        {
            var existingEmployee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (existingEmployee == null) return null;

            existingEmployee.Name = updatedentity.Name;
            existingEmployee.Email = updatedentity.Email;
            existingEmployee.Phone = updatedentity.Phone;
            existingEmployee.DateOfJoin = updatedentity.DateOfJoin;
            existingEmployee.Role = updatedentity.Role;
            existingEmployee.Stack = updatedentity.Stack;

            SetPasswordHash(existingEmployee, updatedentity.Password);

            await _dbContext.SaveChangesAsync();

            // Invalidate cache
            await _cacheService.RemoveAsync($"employee-{id}");

            return existingEmployee;
        }

        public async Task<bool> DeleteEmployee(Guid id)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (employee == null) return false;

            _dbContext.Employees.Remove(employee);
            var result = await _dbContext.SaveChangesAsync() > 0;

            if (result)
            {
                await _cacheService.RemoveAsync($"employee-{id}");
            }

            return result;
        }

        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request)
        {
            var user = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Email == request.Email)
                ?? throw new InvalidOperationException($"User not found with email: {request.Email}");

            var hashedInputPassword = PasswordHasher.HashPassword(request.Password, user.Salt);
            if (user.Password != hashedInputPassword)
            {
                throw new UnauthorizedAccessException("Incorrect password.");
            }

           
            var jwtToken = await _accessTokenService!.GenerateToken(user);

            var refreshToken = RefreshTokenService.GenerateRefreshToken();
            var refreshTokenEntity = new RefreshTokenEntity
            {
                TokenId = Guid.NewGuid(),
                EmployeeId = user.EmployeeId,
                RefreshToken = refreshToken,
                RefreshTokenExpiry = DateTime.UtcNow.AddDays(7)
            };

            await _dbContext.RefreshTokens.AddAsync(refreshTokenEntity);
            await _dbContext.SaveChangesAsync();

            return new AuthenticationResponse
            {
                Id = user.EmployeeId,
                Role = Enum.GetName(typeof(Permissions), user.Role),
                JwToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                RefreshToken = refreshToken
            };
        }

        private static void SetPasswordHash(EmployeeEntity employee, string plainPassword)
        {
            var salt = PasswordHasher.GenerateSalt();
            var hashedPassword = PasswordHasher.HashPassword(plainPassword, salt);

            employee.Password = hashedPassword;
            employee.Salt = salt;
        }
    }
}
