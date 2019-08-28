using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class EmployeeNotWorkReasonStatus
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public decimal PenaltyAmount { get; set; }
        public DateTime signOutDate { get; set; }
        public bool signOut { get; set; }
        public EmployeeNotWorkReason EmployeeNotWorkReason { get; set; }
        public int EmployeeNotWorkReasonId { get; set; }
    }
}
