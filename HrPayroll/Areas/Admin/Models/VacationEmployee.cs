using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class VacationEmployee
    {
        public int Id { get; set; }
        [Required]
        public DateTime StarDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public bool CalcSalary { get; set; } = false;
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}
