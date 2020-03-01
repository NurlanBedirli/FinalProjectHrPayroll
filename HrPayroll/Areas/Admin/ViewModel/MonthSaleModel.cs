using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.ViewModel
{
    public class MonthSaleModel
    {
        public string Emporium { get; set; }
        public string Holding { get; set; }
        public string Company { get; set; }
        public decimal Prize { get; set; }
        public decimal Quata { get; set; }
        public DateTime Date { get; set; }
        public int Id { get; set; }
    }
}
