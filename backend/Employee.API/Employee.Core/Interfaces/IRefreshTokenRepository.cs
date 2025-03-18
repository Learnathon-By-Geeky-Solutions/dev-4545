using Employee.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Core.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<IEnumerable<RefreshTokenEntity>> GetRefreshTokens();
        Task<bool> DeleteToken(Guid id);
        Task<string> GetAccessToken(string refreshToken);
    }
}
