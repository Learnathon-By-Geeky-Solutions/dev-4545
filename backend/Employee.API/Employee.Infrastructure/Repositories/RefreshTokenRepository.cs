using System.IdentityModel.Tokens.Jwt;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Employee.Infrastructure.Data;
using Employee.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Employee.Infrastructure.Repositories
{
    public class RefreshTokenRepository(AppDbContext dbContext, IConfiguration _configuration) : IRefreshTokenRepository
    {
        public async Task<IEnumerable<RefreshTokenEntity>> GetRefreshTokens()
        {
            return await dbContext.RefreshTokens.ToListAsync();

        }
        public async Task<bool> DeleteToken(Guid id)
        {
            var data = await dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.TokenId == id);
            if (data != null)
            {
                dbContext.RefreshTokens.Remove(data);
                return await dbContext.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<string> GetAccessToken(string refreshToken)
        {
            var employee = await dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (employee == null || employee.RefreshTokenExpiry < DateTime.UtcNow)
                throw new Exception("Invalid or expired refresh token.");
            var accessTokenService = new AccessTokenService(_configuration);
            var user = await dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == employee.EmployeeId);
            var JwtSecurity = await accessTokenService.GenerateToken(user);
           // string AccessToken= new JwtSecurityTokenHandler().WriteToken(JwtSecurity);

            return new JwtSecurityTokenHandler().WriteToken(JwtSecurity); ;
        }
    }
}
