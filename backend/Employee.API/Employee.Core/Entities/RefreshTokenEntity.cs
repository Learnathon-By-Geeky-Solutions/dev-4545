using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Core.Entities
{
    public class RefreshTokenEntity
    {
        [Key]
        public Guid TokenId { get; set; }
        public Guid EmployeeId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshTokenExpiry { get; set; }

    }
}
