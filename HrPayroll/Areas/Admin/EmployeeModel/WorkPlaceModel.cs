using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.EmployeeModel
{
    public class WorkPlaceModel
    {
        public int PositionId { get; set; }
        [Required]
        public string HoldingName { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string EmporiaName { get; set; }
        [Required]
        public string PositionName { get; set; }
        [Required]
        public string DepartamentName { get; set; }
        [Required]
        public decimal Salary { get; set; }
        [Required]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        public int EmpPosId { get; set; }
    }
}
