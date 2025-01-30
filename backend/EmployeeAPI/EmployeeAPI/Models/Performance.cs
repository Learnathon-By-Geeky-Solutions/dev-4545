using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI.Models
{
    public class Performance
    {
        [Key]
        public int PerformanceId { get; set; }

        public string Comments { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string Rating{ get; set; }

        public int PersonId { get; set; }
        public int ReviewerId { get; set; }
    }
}
