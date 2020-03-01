using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.AddHolEmpPosDepModel
{
    public class AddCategoryModel
    {
        public int PositionId { get; set; }
        public string HoldingName { get; set; }
        public string CompanyName { get; set; }
        public string EmporiaName { get; set; }
        public string PositionName { get; set; }
        public int DepartamentId { get; set; }
        public string DepartamentName { get; set; }
        [Required]
        public decimal Salary { get; set; }
    }
}
