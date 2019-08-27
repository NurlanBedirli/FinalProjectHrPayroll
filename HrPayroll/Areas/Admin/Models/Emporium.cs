using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class Emporium
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public Company Company { get; set; }
        public int CompanyId { get; set; }
        public ICollection<EmporiumPosition> EmporiumPositions { get; set; }
    }
}
