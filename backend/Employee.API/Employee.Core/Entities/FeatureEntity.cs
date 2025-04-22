using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Core.Entities
{
    public class FeatureEntity
    {
        [Key]
        public Guid FeatureId { get; set; }
        public Guid ProjectId { get; set; }
        public string FeatureName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; } =string.Empty;

    }
}
