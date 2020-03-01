using HrPayroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.EmployeeModel
{
    public class EmployeeChangePositionRol
    {
        public int Id { get; set; }
        public bool CalcSalary { get; set; }
        public DateTime EndDate { get; set; }
        public string Company { get; set; }
        public string Emporium { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
    }
}
