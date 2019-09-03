using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class Penalty
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}
