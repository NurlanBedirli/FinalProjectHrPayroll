using HrPayroll.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class Bonus
    {
        public int Id { get; set; }
        [Required]
        public DateTime BonusDate { get; set; }
        [Required]
        public decimal BonusPrize { get; set; }
        [Required]
        public string BonusStatus { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        
    }
}
