using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.MenecerAttandanceModel
{
    public class EmployeeAttendance
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime AttandanceDate { get; set; }
        public bool SignIn { get; set; }
        public int EmployeeId { get; set; }
    }
}
