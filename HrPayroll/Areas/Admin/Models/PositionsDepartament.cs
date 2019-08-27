using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class PositionsDepartament
    {
        public int Id { get; set; }
        public Positions Positions { get; set; }
        public int PositionsId { get; set; }
        public Departament Departament { get; set; }
        public int DepartamentId { get; set; }
    }
}
