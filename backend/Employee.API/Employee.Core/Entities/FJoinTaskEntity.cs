using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Core.Entities
{
    public class FJoinTaskEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid FeatureId { get; set; }

        public Guid TaskId { get; set; }

    }
}
