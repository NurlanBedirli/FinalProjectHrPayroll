using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrPayroll.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace HrPayroll.Core.SeedRun
{
    public static class Seed
    {
        internal static async Task InvokeAsync(IServiceScope scope, PayrollDbContext payrollDb)
        {
             if(!payrollDb.Users.Any())
            {
                var currentUser = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var role = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var hashedPassword = scope.ServiceProvider.GetRequiredService<IPasswordHasher<AppUser>>();

                AppUser AdminUser = new AppUser()
                {
                    FirstName = "Nurlan",
                    SecondName = "Bedirli",
                    UserName = "NurlanBli",
                    Email = "badirli222@gmail.com"
                };

                AppUser HrUser = new AppUser()
                {
                    FirstName = "Terlan",
                    SecondName = "Usubov",
                    UserName = "Usubov",
                    Email = "usubov@gmail.com",
                    
                };

                AppUser menecer = new AppUser()
                {
                    FirstName = "menecer",
                    SecondName = "menecer",
                    UserName = "menecer",
                    Email = "menecer@gmail.com",
                };


                AdminUser.PasswordHash = hashedPassword.HashPassword(AdminUser, "Nurl@n123");
                HrUser.PasswordHash = hashedPassword.HashPassword(HrUser, "Nurl@n123");
                menecer.PasswordHash = hashedPassword.HashPassword(menecer, "Nurl@n123");

                IdentityResult result =  await currentUser.CreateAsync(AdminUser);
                IdentityResult result1 = await currentUser.CreateAsync(HrUser);
                IdentityResult result2 = await currentUser.CreateAsync(menecer);

                if (result.Succeeded)
                {
                    string[] rols = new string[] { "Admin", "Hr", "Departament", "PayrollSpecialist","Menecer"};
                    foreach(var rol in rols )
                    {
                    IdentityResult identity =   await role.CreateAsync(new IdentityRole { Name = rol});
                    }
                        await currentUser.AddToRoleAsync(AdminUser, "Admin");
                        await currentUser.AddToRoleAsync(HrUser, "Hr");
                        await currentUser.AddToRoleAsync(menecer, "Menecer");
                }
            }
        }
    }
}
