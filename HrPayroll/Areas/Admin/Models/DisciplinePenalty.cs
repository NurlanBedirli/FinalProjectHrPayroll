using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class DisciplinePenalty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PenaltyValue { get; set; }
    }
}
