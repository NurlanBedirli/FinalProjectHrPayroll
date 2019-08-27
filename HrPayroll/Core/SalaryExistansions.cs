using HrPayroll.Areas.Admin.EmployeeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Core
{
    public static class SalaryExistansions
    {
        public static decimal SalaryFindByNumberContainer(this EmployeeInfoWorkPlace salary,int value)
        {
            decimal a = 5;
           if(salary.Salary == (decimal)value)
            {

            }
            return a;
        }
    }
}
