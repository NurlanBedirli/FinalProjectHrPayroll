using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.EmployeeModel
{
    public class EmployeeInfoWorkPlace
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string PlasierCode { get; set; }
        public string CompanyName { get; set; }
        public string EmperiumName { get; set; }
        public string PositionName { get; set; }
        public string DepartamentName { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Salary { get; set; }
    }
}
