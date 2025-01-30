using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeAPI.Models
{
    public class Feature
    {
        [Key]
        public int FeatureId { get; set; }

        public string FeatureName { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string Description { get; set; }

      //  [ForeignKey(nameof(ProjectId))]
        public int ProjectId { get; set; }
    }
}
