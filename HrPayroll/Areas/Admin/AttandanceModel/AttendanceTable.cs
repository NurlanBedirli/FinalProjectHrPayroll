using HrPayroll.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.AttandanceModel
{
    public class AttendanceTable
    {
        public int Id { get; set; }
        public List<SignInTbl> signInlist { get; set; }
        public List<SignInTbl> signOutlist { get; set; }
        public DateTime DateTime { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}
