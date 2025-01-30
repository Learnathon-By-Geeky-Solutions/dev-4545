using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI.Models
{
    public class Leave
    {
        [Key]
        public int LeaveId { get; set; }

        public string LeaveType { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }

        public string Reason { get; set; }
    }
}
