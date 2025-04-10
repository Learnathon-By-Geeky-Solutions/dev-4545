using System.ComponentModel.DataAnnotations;
using Employee.Core.Enums;

namespace Employee.Core.Entities
{
    public class RolesEntity
    {
        [Key]
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string Descriptions { get; set; } = string.Empty;
        public Permissions Permissions {  get; set; }
        
    }
}
