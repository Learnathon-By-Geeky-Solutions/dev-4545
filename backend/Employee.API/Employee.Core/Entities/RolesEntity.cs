using System.ComponentModel.DataAnnotations;
using Employee.Core.Enums;

namespace Employee.Core.Entities
{
    public class RolesEntity
    {
        [Key]
        public Guid RoleId { get; set; }
        public Guid EmployeeId { get; set; }
        public string RoleName { get; set; }
        public string Descriptions { get; set; }
        public Permissions Permissions {  get; set; }
        
    }
}
