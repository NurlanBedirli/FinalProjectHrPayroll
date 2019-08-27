using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class EmployeeAttandance
    {
        public int Id { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
        public EmployeeNotWorkReason NotWorkReason { get; set; }
        public int NotWorkReasonId { get; set; }
    }
}
