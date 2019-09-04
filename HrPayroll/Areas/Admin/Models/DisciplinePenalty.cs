using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class DisciplinePenalty
    {
        public int Id { get; set; }
        [Required]
        public int MinDay { get; set; }
        [Required]
        public int MaxDay { get; set; }
        [Required]
        public int PenaltyValue { get; set; }
    }
}
