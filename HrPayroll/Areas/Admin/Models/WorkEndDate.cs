using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class WorkEndDate
    {
        public int Id { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public bool IsCalcDate { get; set; } = false;
        public decimal Salary { get; set; }
        public string PositionName { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}
