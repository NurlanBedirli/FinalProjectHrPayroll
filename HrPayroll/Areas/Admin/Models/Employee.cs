using HrPayroll.Areas.Admin.EmployeeModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string FatherName { get; set; }
        [Required]
        public string Nationally { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        [MaxLength(12)]
        public string Number { get; set; }
        [Required]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDay { get; set; }
        [Required]
        public string MaritalStatus { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string DistrictRegistration { get; set; }
        [Required]
        public string IDCardSerialNumber { get; set; }
        [Required]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime IDCardExparyDate { get; set; }
        [Required]
        public string PlasiyerCode { get; set; }
        public string Photo { get; set; }

        public List<Education> Educations { get; set; }

        public List<OldWorkPlace> OldWorkPlaces { get; set; }

        public List<Penalty> Penalties { get; set; }

        public List<AbsentCount> AbsentCounts { get; set; }

        public List<SignInOutReasonTbl> SignInOutReasonTbls { get; set; }

        public List<WorkEndDate> WorkEndDates { get; set; }

        public List<WorkPlace> WorkPlaces { get; set; }
    }
}
