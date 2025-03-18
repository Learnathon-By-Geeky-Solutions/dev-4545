using System.ComponentModel.DataAnnotations;

namespace Employee.Core.Entities
{
    public class SalaryEntity
    {
        [Key]
        public Guid SalaryId { get; set; }
        public float Amount { get; set; }
        public DateOnly SalaryDate { get; set; }
        public Guid EmployeeId { get; set; }


    }
}
