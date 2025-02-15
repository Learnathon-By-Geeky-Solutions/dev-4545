using System.ComponentModel.DataAnnotations;

namespace Employee.Core.Entities
{
    public class TaskEntity
    {
        [Key]
        public Guid TaskId { get; set; }
        public string Description { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public Guid AssignedBy { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid FeatureId { get; set; }

    }
}
