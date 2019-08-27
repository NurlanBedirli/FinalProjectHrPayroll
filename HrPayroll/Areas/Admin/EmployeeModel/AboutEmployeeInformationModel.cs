using HrPayroll.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.EmployeeModel
{
    public class AboutEmployeeInformationModel
    {
        public Employee Employees { get; set; }
        public List<Education> Educations { get; set; }
        public List<OldWorkPlace> OldWorkPlaces { get; set; } 
        public EmployeeInfoWorkPlace InfoWorkPlaces { get; set; }
    }
}
