using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Core.Entities
{
    public class PerformanceEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Rating { get; set; }
        public string Comments { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ReviewerId { get; set; }

    }
}
