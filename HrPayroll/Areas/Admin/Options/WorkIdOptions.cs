using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Options
{
    public class WorkIdOptions
    {
        public int? DepartamentId { get; set; }
        public int? CompanyId { get; set; }
        public int? EmporiumId { get; set; }
        public int? PositionsId { get; set; }
        public int? SalaryId { get; set; }
        public int PageSize { get; set; }
    }
}
