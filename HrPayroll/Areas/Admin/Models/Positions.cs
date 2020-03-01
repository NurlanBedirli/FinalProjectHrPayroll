using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class Positions
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public EmployeeSalary EmployeeSalaries { get; set; }
        public ICollection<EmporiumPosition> EmporiumPositions { get; set; }
        public PositionsDepartament PositionsDepartaments { get; set; }
    }
}
