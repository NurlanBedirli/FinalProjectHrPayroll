using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class OficeSalary
    {
        public int Id { get;set; }
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public string RoleManagerId { get; set; }
    }
}
