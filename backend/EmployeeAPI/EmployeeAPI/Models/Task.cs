using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI.Models
{
    public class Task
    {
        [Key]
        public int TaskId { get; set; }

        public string TaskName { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string Description { get; set; }

        //  [ForeignKey]
        public int AssignedTo {  get; set; }
        public int AssignedBy {  get; set; }
        public int FeatureId { get; set; }



     
    }
}
