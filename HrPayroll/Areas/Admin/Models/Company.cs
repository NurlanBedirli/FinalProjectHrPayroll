using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Emporium> Emporia { get; set; }
        public Holding Holding { get; set; }
        public int HoldingId { get; set; }
    }
}
