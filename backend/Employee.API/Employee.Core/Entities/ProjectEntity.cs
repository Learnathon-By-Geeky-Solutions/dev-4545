using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Core.Entities
{
    public class ProjectEntity
    {
        [Key]
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string Client { get; set; }

        
    }
}
