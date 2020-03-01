using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class MonthSale
    {
        public int Id { get; set; }
        [Required]
        public decimal Prize { get; set; }
        [Required]
        public decimal Quata { get; set; }
        [Required]
        public DateTime DateQuata { get; set; }
        public Emporium Emporium { get; set; }
        public int EmporiumId { get; set; }
    }
}
