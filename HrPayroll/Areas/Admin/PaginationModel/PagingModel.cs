using HrPayroll.Areas.Admin.EmployeeModel;
using HrPayroll.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.PaginationModel
{
    public class PagingModel
    {
        public List<Employee> Employees { get; set; }
        public List<EmployeeInfoWorkPlace> EmployeeInfoWorks { get; set; }
        public Paging Paging { get; set; }
    }
}
