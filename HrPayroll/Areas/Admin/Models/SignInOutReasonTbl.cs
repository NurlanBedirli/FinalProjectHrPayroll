using HrPayroll.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class SignInOutReasonTbl
    {
        public int Id { get; set; }
        [Required]
        public string RaasonName { get; set; }
        public string Status { get; set; }
        public decimal PenaltyAmount { get; set; }
        [Required]
        public DateTime SignInTime { get; set; }
        public bool SignIn { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}
