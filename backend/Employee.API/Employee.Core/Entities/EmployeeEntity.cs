using System.ComponentModel.DataAnnotations;

namespace Employee.Core.Entities
{
    public class EmployeeEntity
    {
        [Key]
        public Guid EmployeeId { get; set; }
        public string Name { get; set; } = null!;
        public string Stack { get; set; }=null!;
        public string Password { get; set; }=null!;
        public DateTime DateOfJoin { get; set; }

        public string Email { get; set; } =null!;
        public string Phone { get; set; } = null!;  

           


    }
}
