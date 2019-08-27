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
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PenaltyDate { get; set; }
        public EmployeeNotWorkReason EmployeeNotWorkReason { get; set; }
        public int EmployeeNotWorkReasonId { get; set; }
    }
}
