using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Employee.Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Employee.Infrastructure.Services
{
    public class AccessTokenService
    {
        private readonly IConfiguration _configuration;

        public AccessTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<JwtSecurityToken> GenerateToken(EmployeeEntity employee)
        {
            var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT secret key is not configured.");
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
           // var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, employee.Name),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("EmployeeId", employee.EmployeeId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );

            return token;
        }
    }
}
