using HrPayroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class Bonus
    {
        public int Id { get; set; }
        public DateTime BonusDate { get; set; }
        public decimal BonusPrize { get; set; }
        public string BonusStatus { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        
    }
}
