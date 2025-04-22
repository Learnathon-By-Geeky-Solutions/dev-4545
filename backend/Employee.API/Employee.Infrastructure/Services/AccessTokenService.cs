using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Employee.Application.Interfaces;
using Employee.Core.Entities;
using Employee.Core.Enums;
using Employee.Infrastructure.Setttings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Employee.Infrastructure.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly JwtSettings _jwtSettings;
        
        public AccessTokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<JwtSecurityToken> GenerateToken(EmployeeEntity? employee)
        {
            if (string.IsNullOrEmpty(_jwtSettings.Key))
            {
                throw new InvalidOperationException("JWT secret key is not configured.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, employee.Name),
                new Claim(ClaimTypes.Role, Enum.GetName(typeof(Permissions), employee.Role)),
                new Claim("EmployeeId", employee.EmployeeId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );

            return await Task.FromResult(token);
        }
    }
}
