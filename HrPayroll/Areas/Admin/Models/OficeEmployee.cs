using HrPayroll.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class OficeEmployee
    {
        public int Id { get; set; }
        [Required]
        public DateTime StarDate { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
    }
}
