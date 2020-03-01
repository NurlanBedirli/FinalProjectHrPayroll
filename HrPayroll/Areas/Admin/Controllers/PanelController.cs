using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrPayroll.Models;
using HrPayroll.Areas.Admin.Models;
using HrPayroll.Areas.Admin.EmployeeModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using HrPayroll.Core;
using System.IO;
using HrPayroll.Areas.Admin.BLL;

namespace HrPayroll.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PanelController : Controller
    {
        private readonly PayrollDbContext  dbContext;
        public readonly UserManager<AppUser> userManager;
        public readonly RoleManager<IdentityRole> roleManager;
        public readonly SignInManager<AppUser> signInManager;
        public readonly IPasswordHasher<AppUser> passwordHasher;
        public readonly IPasswordValidator<AppUser> passwordValidator;
        private IHostingEnvironment hostingEnvironment;
        private IFileNameGenerator nameGenerator;


        public PanelController(PayrollDbContext _dbContext,
                           UserManager<AppUser> _userManager, 
                                RoleManager<IdentityRole> _roleManager,
                                    SignInManager<AppUser> _signInManager,
                                       IPasswordHasher<AppUser> _passwordHasher,
                                          IPasswordValidator<AppUser> _passwordValidator,
                                             IHostingEnvironment _hostingEnvironment,
                                                IFileNameGenerator _nameGenerator)
        {
            dbContext = _dbContext;
            userManager = _userManager;
            roleManager = _roleManager;
            signInManager = _signInManager;
            passwordHasher = _passwordHasher;
            passwordValidator = _passwordValidator;
            hostingEnvironment = _hostingEnvironment;
            nameGenerator = _nameGenerator;

        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> MessageRoleList()
        {
            var userRoleModel = await EmployeeChangeRole.GetAppUserAsync(dbContext);
            return View(userRoleModel);
        }

        [HttpPost]
        public async Task<ActionResult> MessageRoleList(AppUserRoleModel userRoleModel)
        {
            var appUserRole = await EmployeeChangeRole.GetAppUserAsync(dbContext);
            try
            {
                Employee employeeEmporium = null;
                AppUser user = null;
                switch (userRoleModel.Role)
                {
                    case "Menecer":
                        if (userRoleModel.Holding != null)
                        {
                            if (userRoleModel.Company != null && userRoleModel.Emporium != 0)
                            {
                                //bele bir sexs varmi
                                employeeEmporium = await EmployeeChangeRole
                                                           .HasIdEmployee(dbContext, userRoleModel.UserId);
                                if (employeeEmporium == null)
                                {
                                    ModelState.AddModelError("", "No such user.");
                                    return View(appUserRole);
                                }


                               //bele bir sexhs hazirda isleyirmi
                               if(employeeEmporium.WorkPlaces != null)
                                {
                                    //bu sexsh isi sonlandiribmi 
                                    var workEnds = await EmployeeChangeRole
                                                                .IsEmployeeWorkEndDate(dbContext, employeeEmporium.Id);
                                    if (!workEnds)
                                        ModelState.AddModelError("", "Hr did not specify the date of his dismissal.");
                                    return View(appUserRole);
                                }


                                //bu magazanin meneceri varmi?
                                var appUser = await EmployeeChangeRole.HasEmporiumAppUser(dbContext, userRoleModel.Emporium);
                                if (appUser)
                                {
                                    ModelState.AddModelError("", "This store already has a manager.");
                                    return View(appUserRole);
                                }


                                //Appuser Create
                                 user = await EmployeeChangeRole.CreateAppUserAsync(dbContext,employeeEmporium,userManager,passwordHasher);


                                if (employeeEmporium.WorkPlaces != null)
                                {
                                    //Change Employee Position Table 
                                    var changepositionrol = EmployeeChangeRole.GetChangePositionRolAsync(dbContext, employeeEmporium.Id, user.Id);
                                }


                                //ofice employee work start date
                                OficeEmployee oficeEmployeee = new OficeEmployee()
                                {
                                    StarDate = DateTime.Now,
                                    AppUserId = user.Id
                                };
                                dbContext.OficeEmployees.Add(oficeEmployeee);
                                await dbContext.SaveChangesAsync();


                                //menecer emporium id
                                EmporiumAppUserMenecer emporiumAppUser = new EmporiumAppUserMenecer()
                                {
                                    AppUserId = user.Id,
                                    EmporiumId = userRoleModel.Emporium
                                };
                                await dbContext.EmporiumAppUsers.AddAsync(emporiumAppUser);
                                await dbContext.SaveChangesAsync();

                                //user  rol bagla
                                await  userManager.AddToRoleAsync(user, userRoleModel.Role);


                                var currentMessagee = await dbContext
                                             .MessageReciurments
                                                    .Where(x => x.EmployeeId == employeeEmporium.Id).FirstOrDefaultAsync();
                                var data1 = dbContext.Employees
                                                  .Where(x => x.Id == employeeEmporium.Id)
                                                           .FirstOrDefault();

                                dbContext.MessageReciurments.Remove(currentMessagee);
                                dbContext.Employees.Remove(data1);
                                await dbContext.SaveChangesAsync();
                                ModelState.AddModelError("", "Success");
                                return View(appUserRole);
                            }
                            ModelState.AddModelError("", "Company or Emporium is empty");
                            return View(appUserRole);
                        }
                        ModelState.AddModelError("", "Holding is empty");
                        return View(appUserRole);
                }



                bool exists = await roleManager.RoleExistsAsync(userRoleModel.Role);
                if (!exists)
                {
                    ModelState.AddModelError("", "There is no such role.");
                    return View(appUserRole);
                }


                //bele bir sexs varmi
                employeeEmporium = await EmployeeChangeRole
                                           .HasIdEmployee(dbContext, userRoleModel.UserId);
                if (employeeEmporium == null)
                {
                    ModelState.AddModelError("", "No such User.");
                    return View(appUserRole);
                }


                //bele bir sexhs hazirda isleyirmi
                if (employeeEmporium.WorkPlaces != null)
                {
                    //bu sexsh isi sonlandiribmi 
                    var workEnds = await EmployeeChangeRole
                                                .IsEmployeeWorkEndDate(dbContext, employeeEmporium.Id);
                    if (!workEnds)
                        ModelState.AddModelError("", "Hr did not specify the date of his dismissal.");
                    return View(appUserRole);
                }


                //Appuser Create
                user = await EmployeeChangeRole.CreateAppUserAsync(dbContext, employeeEmporium, userManager, passwordHasher);


                if (employeeEmporium.WorkPlaces != null)
                {
                    //Change Employee Position Table 
                    var changepositionrol = EmployeeChangeRole.GetChangePositionRolAsync(dbContext, employeeEmporium.Id, user.Id);
                }


                   OficeEmployee oficeEmployee = new OficeEmployee()
                    {
                     StarDate = DateTime.Now,
                     AppUserId = user.Id
                    };
                    dbContext.OficeEmployees.Add(oficeEmployee);
                    await dbContext.SaveChangesAsync();

                    //user add to role
                   await userManager.AddToRoleAsync(user, userRoleModel.Role);


                var currentMessage = await dbContext.MessageReciurments.Where(x => x.EmployeeId == employeeEmporium.Id).FirstOrDefaultAsync();
                    dbContext.MessageReciurments.Remove(currentMessage);
                    var data = dbContext.Employees
                                                 .Where(x => x.Id == employeeEmporium.Id)
                                                          .FirstOrDefault();
                    dbContext.Employees.Remove(data);
                    await dbContext.SaveChangesAsync();
            }
            catch(Exception exp)
            {
                ModelState.AddModelError("", exp.Message);
            }
            return View(appUserRole);
        }

        [HttpGet]
        public async Task<ActionResult> DeleteMessageRole(int? id)
        {
            if(id.HasValue)
            {
                var data =  dbContext.MessageReciurments.Where(x => x.EmployeeId == id).FirstOrDefault();
                if(data != null)
                {
                    dbContext.MessageReciurments.Remove(data);
                    await dbContext.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(MessageRoleList));
        }

        [HttpGet]
        public async Task<ActionResult> AppUserList()
        {
            List<AppUserRoleModel> roleModels = new List<AppUserRoleModel>();
            List<AppUser> user = userManager.Users.ToList();
            foreach(var elm in user)
            {
                var data = await  userManager.GetRolesAsync(elm);
                AppUserRoleModel roleModel = new AppUserRoleModel()
                {
                    AppUser = elm,
                    Roles = data.ToList()
                };
                roleModels.Add(roleModel);
            }
            return View(roleModels);
        }

        [HttpGet]
        public async Task<ActionResult> EditAppUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            AppUser currentUser = dbContext.Users.Where(x => x.Id == id).FirstOrDefault();
            HttpContext.Session.SetObjectAsJson("UserId",id);
            AdminModel model = new AdminModel()
            {
                FirstName = currentUser.FirstName,
                SecondName = currentUser.SecondName,
                Address = currentUser.Adress,
                Birthday = currentUser.Birth,
                Email = currentUser.Email,
                PhoneNumber = currentUser.PhoneNumber,
                Image = currentUser.Photo
            };
            await Task.Delay(0);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAppUser(AdminModel adminModel, IFormFile Image)
        {
            string Id = HttpContext.Session.GetObjectFromJson<string>("UserId");
            AppUser user = userManager.Users.Where(x => x.Id == Id).First();
            user.PhoneNumber = adminModel.PhoneNumber;
            user.FirstName = adminModel.FirstName;
            user.SecondName = adminModel.SecondName;
            user.Birth = adminModel.Birthday.ToUniversalTime();
            user.Adress = adminModel.Address;
            if (ModelState.IsValid)
            {
                if (adminModel.OldPassword != null)
                {
                    IList<string> str = await userManager.GetRolesAsync(user);
                    PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, adminModel.OldPassword);
                    if (PasswordVerificationResult.Success == result)
                    {
                        if (Image != null)
                        {
                            if (Image.IsFilePhotoFormat())
                            {
                                string fullPath = hostingEnvironment.GetFolder();
                                string format = Image.GetFileFormat();
                                string fileName = nameGenerator.GetFileName(format);
                                string fullFilePath = Path.Combine(fullPath, fileName);
                                await Image.SaveFileAsync(fullFilePath);
                                user.Photo = fileName;
                                ImageRemove.PhotoPathDelete(user.Photo, fullPath);
                                if (adminModel.NewPassword == null)
                                {
                                    ModelState.AddModelError("", "New Password empty");
                                }
                                else
                                {
                                    user.PasswordHash = passwordHasher.HashPassword(user, adminModel.NewPassword);
                                    dbContext.Update<AppUser>(user);
                                    await dbContext.SaveChangesAsync();
                                    ModelState.AddModelError("", "Succes");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Photo is not Format and format (jpg,png)");
                            }
                        }
                        else
                        {
                            if (adminModel.NewPassword == null)
                            {
                                ModelState.AddModelError("", "New Password empty");
                            }
                            else
                            {
                                user.PasswordHash = passwordHasher.HashPassword(user, adminModel.NewPassword);
                                dbContext.Update<AppUser>(user);
                                await dbContext.SaveChangesAsync();
                                ModelState.AddModelError("", "Succes");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Old Password Incorrect");
                    }
                }
                else
                {
                    IList<string> str = await userManager.GetRolesAsync(user);
                    if (Image != null)
                    {
                        if (Image.IsFilePhotoFormat())
                        {
                            string fullPath = hostingEnvironment.GetFolder();
                            string format = Image.GetFileFormat();
                            string fileName = nameGenerator.GetFileName(format);
                            string fullFilePath = Path.Combine(fullPath, fileName);
                            ImageRemove.PhotoPathDelete(user.Photo, fullPath);
                            await Image.SaveFileAsync(fullFilePath);
                            user.Photo = fileName;
                          
                            dbContext.Update<AppUser>(user);
                            await dbContext.SaveChangesAsync();
                            ModelState.AddModelError("", "Succes");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Photo is not Format and format (jpg,png)");
                        }
                    }
                    else
                    {
                        dbContext.Update<AppUser>(user);
                        await dbContext.SaveChangesAsync();
                        ModelState.AddModelError("", "Succes");
                    }
                }
            }
            return View(adminModel);
        }

        [HttpGet]
        public ActionResult AddDisciplinePenalty()
        {
            ViewBag.Penalty = dbContext.DisciplinePenalties.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddDisciplinePenalty(DisciplinePenalty penalty)
        {
            if(ModelState.IsValid)
            {
                var data = dbContext.DisciplinePenalties.ToList();
                if(data.Count < 1 && penalty.PenaltyValue != 0)
                {
                    DisciplinePenalty disciplinePenalty = new DisciplinePenalty
                    {
                        PenaltyValue = penalty.PenaltyValue,
                        MinDay = penalty.MinDay,
                        MaxDay = penalty.MaxDay
                    };
                    dbContext.DisciplinePenalties.Add(disciplinePenalty);
                    dbContext.SaveChanges();
                }
                ModelState.AddModelError("", "discipline can only be added once or ");
            }
            ViewBag.Penalty = dbContext.DisciplinePenalties.ToList();
            return View();
        }

        [HttpGet]
        public ActionResult DeleteDiscipline(int? id)
        {
            if(id.HasValue)
            {
                var discipline = dbContext.DisciplinePenalties.Where(x => x.Id == id).FirstOrDefault();
                if(discipline !=null)
                {
                    dbContext.DisciplinePenalties.Remove(discipline);
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction(nameof(AddDisciplinePenalty));
        }

        [HttpGet]
        public async Task<ActionResult> Dismissed()
        {
            var data = dbContext.Dismisseds.Skip(5).Take(10).ToList();
            await Task.Delay(0);
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ChangeRol(string id)
        {
            if(!string.IsNullOrEmpty(id))
            {
                AppUserRoleModel userRoleModel =  await  EmployeeChangeRole.UserRoleAsync(dbContext,id,userManager,roleManager);
                HttpContext.Session.SetObjectAsJson("userId", userRoleModel.AppUser);
                return View(userRoleModel);

            }
            return RedirectToAction(nameof(AppUserList));
        }

        [HttpPost]
        public async Task<ActionResult> ChangeRol(AppUserRoleModel userRoleModel)
        {
           AppUser appUserr = HttpContext.Session.GetObjectFromJson<AppUser>("userId");
            try
            {
                AppUser user = null;
                switch (userRoleModel.Role)
                {
                    case "Menecer":
                        if (userRoleModel.Holding != null)
                        {
                            if (userRoleModel.Company != null && userRoleModel.Emporium != 0)
                            {
                                //bele bir sexs varmi
                                 user =  dbContext.Users.Where(x => x.Id == userRoleModel.AppUserId).FirstOrDefault();

                                if (user == null)
                                {
                                    return RedirectToAction(nameof(AppUserList));
                                }


                                //bu magazanin meneceri varmi?
                                var appUser = await EmployeeChangeRole.HasEmporiumAppUser(dbContext, userRoleModel.Emporium);
                                if (appUser)
                                {
                                    ModelState.AddModelError("", "This store already has a manager.");
                                    return RedirectToAction(nameof(AppUserList));
                                }


                                var IsRole =  await userManager.IsInRoleAsync(user, userRoleModel.Role);
                                if(IsRole)
                                {
                                    //menecer emporium id
                                    EmporiumAppUserMenecer emporiumAppUser = new EmporiumAppUserMenecer()
                                    {
                                        AppUserId = userRoleModel.AppUserId,
                                        EmporiumId = userRoleModel.Emporium
                                    };
                                    await dbContext.EmporiumAppUsers.AddAsync(emporiumAppUser);
                                    await dbContext.SaveChangesAsync();
                                }
                                else
                                {
                                    await  userManager.AddToRoleAsync(user, userRoleModel.Role);

                                    //menecer emporium id
                                    EmporiumAppUserMenecer emporiumAppUser = new EmporiumAppUserMenecer()
                                    {
                                        AppUserId = userRoleModel.AppUserId,
                                        EmporiumId = userRoleModel.Emporium
                                    };
                                    await dbContext.EmporiumAppUsers.AddAsync(emporiumAppUser);
                                    await dbContext.SaveChangesAsync();
                                }

                                return RedirectToAction(nameof(AppUserList));
                            }
                            return RedirectToAction(nameof(AppUserList));
                        }
                        return RedirectToAction(nameof(AppUserList));
                }


                bool exists = await roleManager.RoleExistsAsync(userRoleModel.Role);
                if (!exists)
                {
                    return RedirectToAction(nameof(AppUserList));
                }

                //bele bir sexs varmi
                user = dbContext.Users.Where(x => x.Id == userRoleModel.AppUserId).FirstOrDefault();

                if (user == null)
                {
                    return RedirectToAction(nameof(AppUserList));
                }

                if(!await userManager.IsInRoleAsync(user,userRoleModel.Role))
                {
                    await userManager.AddToRoleAsync(user, userRoleModel.Role);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError("", "This user already has a role");
                }
               
            }
            catch (Exception exp)
            {
                ModelState.AddModelError("", exp.Message);
            }

              return RedirectToAction("ChangeRol", "Panel", new { id = userRoleModel.AppUserId });
        }

        [HttpGet]
        public async Task<ActionResult> DeleteAppUserRol(string Roleid)
        {
            var user = HttpContext.Session.GetObjectFromJson<AppUser>("userId");

            if (!string.IsNullOrWhiteSpace(Roleid))
            {

                try
                {
                    var role = await roleManager.FindByIdAsync(Roleid);
                    var appuser = await userManager.FindByIdAsync(user.Id);
                    if (role != null)
                    {
                        if (await userManager.IsInRoleAsync(user, role.Name))
                        {
                            if (role.Name == "Menecer")
                            {
                                var data = dbContext.EmporiumAppUsers.Where(x => x.AppUserId == user.Id).FirstOrDefault();
                                dbContext.EmporiumAppUsers.Remove(data);
                                await userManager.RemoveFromRoleAsync(user, role.Name);
                                await dbContext.SaveChangesAsync();
                            }
                            else
                            {
                                var result = await userManager.RemoveFromRoleAsync(appuser, role.Name);
                        }

                        }
                    }
                }
                catch(Exception exp)
                {
                    ModelState.AddModelError("", exp.Message);
                }
                
            }
            return RedirectToAction("ChangeRol", "Panel", new { id = user.Id });
        }

    }
}