using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.EmployeeModel
{
    public class MenecerEmployeeBonusModel
    {
        public string Name { get; set; }
        public string Emporium { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; } 
        public decimal Bonus { get; set; }
        public int EmployeeId { get; set; }
    }
}
