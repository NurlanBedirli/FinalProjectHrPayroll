using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class AdminModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }

        public string Email { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
        [Required]
        [Phone]
        [MaxLength(12)]
        public string PhoneNumber { get; set; }

        public DateTime Birthday { get; set; }

        public string Address { get; set; }

        public string Image { get; set; }

        public string Role { get; set; }
    }
}
