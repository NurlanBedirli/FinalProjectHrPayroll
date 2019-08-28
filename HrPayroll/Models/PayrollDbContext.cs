using HrPayroll.Areas.Admin.AttandanceModel;
using HrPayroll.Areas.Admin.EmployeeModel;
using HrPayroll.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Models
{
    public class PayrollDbContext : IdentityDbContext<AppUser>
    {
        public PayrollDbContext(DbContextOptions<PayrollDbContext> options) : base(options) { }

        public DbSet<Education> Educations { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<MaritalStatus> MaritalStatuses { get; set; }
        public DbSet<OldWorkPlace> OldWorkPlaces { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Holding> Holdings { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Emporium> Emporia { get; set; }
        public DbSet<Positions> Positions { get; set; }
        public DbSet<EmployeeSalary> EmployeeSalaries { get; set; }
        public DbSet<Departament> Departaments { get; set; }
        public DbSet<PositionsDepartament> PositionsDepartaments { get; set; }
        public DbSet<EmporiumPosition> EmporiumPositions { get; set; }
        public DbSet<WorkPlace> Placeswork { get; set; }
        public DbSet<WorkEndDate> WorkEnds { get;set; }
        public DbSet<EmployeeNotWorkReason> NotWorkReasons { get; set; }
        public DbSet<EmployeeNotWorkReasonStatus> WorkReasonStatuses { get; set; }
        public DbSet<EmporiumAppUserMenecer> EmporiumAppUsers { get; set; }
        public DbSet<EmployeeAttandance> EmployeeAttandances { get; set; }
        public DbSet<SignInTbl> SignInTbls { get; set; }
    }
}
