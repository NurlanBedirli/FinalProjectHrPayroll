using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class Holding
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Company> Companies { get; set; }
        public List<Departament> Departaments { get; set; }
    }
}
