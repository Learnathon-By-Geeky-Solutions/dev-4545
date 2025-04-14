using System.ComponentModel.DataAnnotations;

namespace Employee.Core.Entities
{
    public class TaskEntity
    {
        [Key]
        public Guid TaskId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid AssignedBy { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid FeatureId { get; set; }

    }
}
