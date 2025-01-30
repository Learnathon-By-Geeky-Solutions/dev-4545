using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI.Models
{
    public class Salary
    {
        [Key] public int SalaryId { get; set; }
        public string Amount { get; set; }
        public DateTime? ProvidedDate { get; set; }
        public int EmployeeId { get; set; }

    }
}
