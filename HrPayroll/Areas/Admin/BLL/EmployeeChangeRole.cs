using HrPayroll.Areas.Admin.EmployeeModel;
using HrPayroll.Areas.Admin.Models;
using HrPayroll.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.BLL
{
    public static class EmployeeChangeRole
    {
        

         public static async Task<Employee> HasIdEmployee(PayrollDbContext dbContext,int UserId)
        {
           Employee employee = await dbContext.Employees.Where(x => x.Id == UserId)
                                                    .Include(x => x.WorkPlaces)
                                                        .ThenInclude(x => x.Emporium)
                                                             .FirstOrDefaultAsync();
            return employee;
        }


        public  static  async Task<bool> IsEmployeeWorkEndDate(PayrollDbContext dbContext,int empId)
        {
           bool found =   await dbContext
                                    .WorkEnds
                                         .AnyAsync(x => x.EmployeeId == empId);
            return found;
        }


        public static async Task<bool> HasEmporiumAppUser(PayrollDbContext dbContext,int emporiumId)
        {
          bool found = await dbContext
                              .EmporiumAppUsers
                                      .AnyAsync(x => x.EmporiumId == emporiumId);
            return found;
        }


        public static async Task<AppUser> CreateAppUserAsync(PayrollDbContext dbContext
                                                                    ,Employee employee,
                                                                          UserManager<AppUser> userManager,
                                                                               IPasswordHasher<AppUser> passwordHasher)
        {
           AppUser user = new AppUser()
            {
                FirstName = employee.Name,
                SecondName = employee.Surname,
                Photo = employee.Photo,
                Adress = employee.DistrictRegistration,
                Birth = employee.BirthDay,
                UserName = employee.Name,
                Email = employee.Email,
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "12345");
            await userManager.CreateAsync(user);
            await dbContext.SaveChangesAsync();

            return user;
        }


        public static async Task<EmployeeChangePositionRol> GetChangePositionRolAsync(PayrollDbContext dbContext,
                                                                                           int employeeId,
                                                                                                    string appuserId)
        {
            var changepositionrol = dbContext.Placeswork.Where(x => x.EmployeeId == employeeId)
                                     .Include(x => x.Employee)
                                       .ThenInclude(x => x.WorkEndDates)
                                        .Include(x => x.Emporium)
                                          .ThenInclude(x => x.Company)
                                             .Include(x => x.Positions)
                                                .ThenInclude(x => x.EmployeeSalaries)
                                                   .Select(x => new EmployeeChangePositionRol
                                                   {
                                                       Emporium = x.Emporium.Name,
                                                       Position = x.Positions.Name,
                                                       Salary = x.Positions.EmployeeSalaries.Salary,
                                                       Company = x.Emporium.Company.Name,
                                                       CalcSalary = false,
                                                       EndDate = x.Employee.WorkEndDates.First().EndDate,
                                                       AppUserId = appuserId
                                                   }).FirstOrDefault();
            dbContext.ChangePositionRols.Add(changepositionrol);
            await dbContext.SaveChangesAsync();

            return changepositionrol;
        }


        public static async Task<AppUserRoleModel> GetAppUserAsync(PayrollDbContext dbContext)
        {
            var data = await dbContext.MessageReciurments
                  .ToListAsync();

            var role = dbContext.Roles
                    .ToList();
            var holdings = dbContext.Holdings.ToList();

            AppUserRoleModel userRoleModel = new AppUserRoleModel()
            {
                MessageReciurments = data,
                Holdings = holdings,
                IdentityAllRole = role
            };
            return userRoleModel;
        }


        public static async Task<AppUserRoleModel> UserRoleAsync(PayrollDbContext dbContext,string userid,
                                                                    UserManager<AppUser> userManager,
                                                                        RoleManager<IdentityRole> roleManager)
        {
            var user = await dbContext.Users
                                            .Where(x => x.Id == userid)
                                                 .FirstOrDefaultAsync();
            AppUserRoleModel userRoleModel = null;
            if (user != null)
            {
                List<IdentityRole> identityRoles = new List<IdentityRole>();
                var data = await userManager
                                      .GetRolesAsync(user);

                foreach(var rol in data)
                {
                   var rolee = roleManager.Roles.Where(x => x.Name == rol).FirstOrDefault();
                    identityRoles.Add(rolee);
                }

                var role = dbContext
                                    .UserRoles.Where(x=> x.UserId == userid)
                                                                        .ToList();

                var rols = dbContext.Roles
                    .ToList();

                var holdings = dbContext
                                       .Holdings
                                            .ToList();


                 userRoleModel = new AppUserRoleModel()
                {
                    AppUser = user,
                    Holdings = holdings,
                    IdentityUserRoles = identityRoles,
                    IdentityAllRole = rols 
                 };
            }
            return userRoleModel;
        }

    }
}
