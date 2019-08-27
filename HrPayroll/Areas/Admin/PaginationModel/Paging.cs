using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.PaginationModel
{
    public class Paging
    {
        public int ItemPage { get; set; } = 9;
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount 
        {
            get
            {
                return (int)Math.Ceiling(TotalItems / (decimal)ItemPage);
            }
        }
        public bool Prev { get; set; }
        public bool Next { get; set; }
    }
}
