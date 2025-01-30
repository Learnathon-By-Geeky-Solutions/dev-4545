using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string Description { get; set; }

        public string Client {  get; set; }

    }
}
