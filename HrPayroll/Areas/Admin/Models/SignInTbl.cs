using HrPayroll.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class SignInTbl
    {
        public int Id { get; set; }
        public DateTime SignInTime { get; set; }
        public bool SignIn { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}
