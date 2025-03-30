using System.ComponentModel.DataAnnotations;
using Employee.Core.Enums;

namespace Employee.Core.Entities
{
    public class LeaveEntity
    {

        [Key]
        public Guid LeaveId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string LeaveType { get; set; }
        public Status Status { get; set; }
        public string Reason { get; set; }
        public Guid EmployeeId { get; set; }

    }
}
