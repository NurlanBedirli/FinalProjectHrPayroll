using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class AbsentCount
    {
        public int Id { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}
