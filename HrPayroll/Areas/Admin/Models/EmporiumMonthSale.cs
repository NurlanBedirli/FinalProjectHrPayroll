using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class EmporiumMonthSale
    {
        public int Id { get; set; }
        [Required]
        public decimal Prize { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public Emporium Emporium { get; set; }
        public int EmporiumId { get; set; }
    }
}
