using HrPayroll.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.AddHolEmPosDepModel
{
    public class HolEmpPosModel
    {
        public Holding Holding { get; set; }
        public Emporium Emporium { get; set; }
        public Company Company { get; set; }
        public Positions Positions { get; set; }
        public Departament Departament { get; set; }
        public List<Holding> Holdings { get; set; }
        public List<Emporium> Emporias { get; set; }
        public List<Company> Companies { get; set; }
        public List<Positions> Positionss { get; set; }
        public List<Departament> Departaments { get; set; }
    }
}
