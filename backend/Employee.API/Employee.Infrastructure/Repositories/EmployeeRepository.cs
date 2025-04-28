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
    public class EmployeeRepository(
        AppDbContext dbContext,
        ICacheService cacheService,
        IConfiguration configuration,
        IAccessTokenService accessTokenService) : IEmployeeRepository
    {
        private const int CacheExpirationInSeconds = 30;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly ICacheService _cacheService = cacheService;
        private readonly IConfiguration _configuration = configuration;
        private readonly IAccessTokenService? _accessTokenService = accessTokenService;

        public async Task<IEnumerable<EmployeeEntity>> GetEmployees()
            => await _dbContext.Employees.ToListAsync();

        public async Task<EmployeeEntity?> GetEmployeeById(Guid id)
            => await GetEmployeeFromCacheOrDb(id);

        public async Task<EmployeeEntity> AddEmployee(EmployeeEntity employee)
        {
            InitializeEmployee(employee);
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<EmployeeEntity?> UpdateEmployee(Guid id, EmployeeEntity updatedEntity)
        {
            var existingEmployee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (existingEmployee == null)
                return null;

            UpdateEmployeeFields(existingEmployee, updatedEntity);
            await _dbContext.SaveChangesAsync();
            await InvalidateEmployeeCache(id);
            return existingEmployee;
        }

        public async Task<bool> DeleteEmployee(Guid id)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (employee == null)
                return false;

            _dbContext.Employees.Remove(employee);
            var result = await _dbContext.SaveChangesAsync() > 0;
            if (result)
                await InvalidateEmployeeCache(id);

            return result;
        }

        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request)
        {
            var user = await FindUserByEmail(request.Email!);
            ValidatePassword(user, request.Password!);

            var jwtToken = await GenerateJwtToken(user);
            var refreshToken = await GenerateAndStoreRefreshToken(user);

            return BuildAuthenticationResponse(user, jwtToken, refreshToken);
        }

        // --- Private Helpers ---

        private async Task<EmployeeEntity?> GetEmployeeFromCacheOrDb(Guid id)
        {
            var cacheKey = GetEmployeeCacheKey(id);
            var cachedEmployee = await _cacheService.GetAsync<EmployeeEntity>(cacheKey);
            if (cachedEmployee != null)
                return cachedEmployee;

            var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (employee != null)
                await _cacheService.SetAsync(cacheKey, employee, TimeSpan.FromSeconds(CacheExpirationInSeconds));

            return employee;
        }

        private static void InitializeEmployee(EmployeeEntity employee)
        {
            employee.EmployeeId = Guid.NewGuid();
            SetPasswordHash(employee, employee.Password);
        }

        private static void UpdateEmployeeFields(EmployeeEntity existingEmployee, EmployeeEntity updatedEntity)
        {
            existingEmployee.Name = updatedEntity.Name;
            existingEmployee.Email = updatedEntity.Email;
            existingEmployee.Phone = updatedEntity.Phone;
            existingEmployee.DateOfJoin = updatedEntity.DateOfJoin;
            existingEmployee.Role = updatedEntity.Role;
            existingEmployee.Stack = updatedEntity.Stack;
            SetPasswordHash(existingEmployee, updatedEntity.Password);
        }

        private static void SetPasswordHash(EmployeeEntity employee, string plainPassword)
        {
            var salt = PasswordHasher.GenerateSalt();
            var hashedPassword = PasswordHasher.HashPassword(plainPassword, salt);
            employee.Password = hashedPassword;
            employee.Salt = salt;
        }

        private static string GetEmployeeCacheKey(Guid id)
            => $"employee-{id}";

        private async Task InvalidateEmployeeCache(Guid id)
            => await _cacheService.RemoveAsync(GetEmployeeCacheKey(id));

        private async Task<EmployeeEntity> FindUserByEmail(string email)
            => await _dbContext.Employees.FirstOrDefaultAsync(x => x.Email == email)
               ?? throw new InvalidOperationException($"User not found with email: {email}");

        private static void ValidatePassword(EmployeeEntity user, string inputPassword)
        {
            var hashedInputPassword = PasswordHasher.HashPassword(inputPassword, user.Salt);
            if (user.Password != hashedInputPassword)
                throw new UnauthorizedAccessException("Incorrect password.");
        }

        private async Task<JwtSecurityToken> GenerateJwtToken(EmployeeEntity user)
            => await _accessTokenService!.GenerateToken(user);

        private async Task<string> GenerateAndStoreRefreshToken(EmployeeEntity user)
        {
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
            return refreshToken;
        }

        private static AuthenticationResponse BuildAuthenticationResponse(EmployeeEntity user, JwtSecurityToken jwtToken, string refreshToken)
            => new()
            {
                Id = user.EmployeeId,
                Role = Enum.GetName(typeof(Permissions), user.Role),
                JwToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                RefreshToken = refreshToken
            };
    }
}
