using HrPayroll.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.EmployeeModel
{
    public class Education
    {
        public int Id { get; set; }
        [Required]
        public string UniversityName { get; set; }
        [Required]
        public string UniversityFaculty { get; set; }
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        //Hal hazirda oxuyuram ve ya ise gore saxlamisam
        public string Status { get; set; }

        public Employee Employee { get; set; }

        public int? EmployeeId { get; set; }
    }
}
