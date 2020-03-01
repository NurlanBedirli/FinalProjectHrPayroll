using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.ViewModel
{
    public class AppUserRole
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Number { get; set; }
        public DateTime BirthDate { get; set; }
        public string Role { get; set; }
    }
}
