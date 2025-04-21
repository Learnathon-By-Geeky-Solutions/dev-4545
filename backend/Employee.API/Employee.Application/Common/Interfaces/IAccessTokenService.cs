using System.IdentityModel.Tokens.Jwt;
using Employee.Core.Entities;

namespace Employee.Application.Interfaces
{
    public interface IAccessTokenService
    {
        Task<JwtSecurityToken> GenerateToken(EmployeeEntity? employee);
    }
}
