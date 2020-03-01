using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class EmployeAccuredSalary
    {
        public int Id { get; set; }
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public DateTime AccuredDate { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}
