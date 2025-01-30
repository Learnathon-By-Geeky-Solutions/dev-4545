using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI.Models
{
    public class Role
    {
        
        [Key] public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Descripiton { get; set; }
        public string Permissions { get; set; }
        //[ForeignKey]
        public int EmployeeId { get; set; }

    }
}
