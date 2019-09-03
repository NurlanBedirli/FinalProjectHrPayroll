using HrPayroll.Areas.Admin.EmployeeModel;
using HrPayroll.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor httpContext;

        public PayrollDbContext(DbContextOptions<PayrollDbContext> options, IHttpContextAccessor _httpContext) : base(options)
        {
            httpContext = _httpContext;
        }

        //Database Model Table
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
        public DbSet<EmporiumAppUserMenecer> EmporiumAppUsers { get; set; }
        public DbSet<SignInOutReasonTbl> SignInOutReasons { get; set; }
        public DbSet<Penalty> Penalties { get; set; }
        public DbSet<AbsentCount> AbsentCounts { get; set; }
        public DbSet<DisciplinePenalty> DisciplinePenalties { get; set; }

        //Fluent API
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Education>()
                  .HasOne(x => x.Employee)
                    .WithMany(y => y.Educations)
                      .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OldWorkPlace>()
                  .HasOne(x => x.Employee)
                    .WithMany(y => y.OldWorkPlaces)
                      .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AbsentCount>()
                 .HasOne(x => x.Employee)
                   .WithMany(y => y.AbsentCounts)
                     .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Penalty>()
                 .HasOne(x => x.Employee)
                   .WithMany(y => y.Penalties)
                     .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SignInOutReasonTbl>()
                .HasOne(x => x.Employee)
                  .WithMany(y => y.SignInOutReasonTbls)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<WorkEndDate>()
                .HasOne(x => x.Employee)
                  .WithMany(y => y.WorkEndDates)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<WorkPlace>()
             .HasOne(x => x.Employee)
               .WithMany(y => y.WorkPlaces)
                 .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }
    }
}
