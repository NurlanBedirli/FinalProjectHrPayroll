using HrPayroll.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class AppUserRoleModel
    {
        public List<MessageReciurment> MessageReciurments { get; set; }
        public List<Holding> Holdings { get; set; }
        public AppUser AppUser { get; set; }
        public string Role { get; set; }
        public string Company { get; set; }
        public int Emporium { get; set; }
        public string Holding { get; set; }
        public List<string> Roles { get; set; }
        public List<IdentityRole> IdentityUserRoles { get; set; }
        public List<IdentityRole> IdentityAllRole { get; set; }
        public int UserId { get; set; }
        public string AppUserId { get; set; }
    }
}
