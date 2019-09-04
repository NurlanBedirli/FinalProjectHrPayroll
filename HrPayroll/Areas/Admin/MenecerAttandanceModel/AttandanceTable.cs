using HrPayroll.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.MenecerAttandanceModel
{
    public class AttandanceTable
    {
        public List<List<EmployeeAttendance>> attendances { get; set; }
        public List<EmployeeAttendance> employeeAttendances { get; set; }
        public List<AbsentCount> AbsentCount { get; set; }
        public DisciplinePenalty DisciplinePenalty { get; set; }
    }
}
