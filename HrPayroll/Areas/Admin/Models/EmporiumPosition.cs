using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class EmporiumPosition
    {
        public int Id { get; set; }
        public Emporium Emporium { get; set; }
        public int EmporiumId { get; set; }
        public Positions Positions { get; set; }
        public int PositionsId { get; set; }
    }
}
