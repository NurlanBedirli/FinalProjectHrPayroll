using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage ="UserName or Email is empty")]
        public string UserNameOrEmail { get; set; }
        [Required(ErrorMessage ="Password is empty")]
        public string Password { get; set; }
    }
}
