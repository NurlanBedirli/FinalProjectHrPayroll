using HrPayroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class EmporiumAppUserMenecer
    {
        public int Id { get; set; }
        public Emporium Emporium { get; set; }
        public int EmporiumId { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
    }
}
