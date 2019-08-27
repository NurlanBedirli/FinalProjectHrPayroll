using HrPayroll.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.EmployeeModel
{
    public class EmployeeHoldingModel
    {
        public List<Holding> Holdings { get; set; }
        public Employee Employees { get; set; } 
        public WorkPlaceModel PlaceModel { get; set; }
    }
}
