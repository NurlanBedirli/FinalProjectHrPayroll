using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class EmployeeSalary
    {
        public int Id { get; set; }
        public decimal Salary { get; set; }
        public Positions Positions { get; set; }
        public int PositionsId { get; set; }
    }
}
