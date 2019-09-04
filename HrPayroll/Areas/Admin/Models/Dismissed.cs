using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class Dismissed
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public string DistrictRegistration { get; set; }
        public string IDCardSerialNumber { get; set; }
        public string PlasiyerCode { get; set; }
        public string Photo { get; set; }
    }
}
