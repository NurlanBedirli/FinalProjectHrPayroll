using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HrPayroll.Areas.Admin.AddHolEmPosDepModel;
using HrPayroll.Areas.Admin.Models;
using HrPayroll.Areas.Admin.PaginationModel;
using HrPayroll.Core;
using HrPayroll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HrPayroll.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        public readonly UserManager<AppUser> userManager;
        public readonly RoleManager<IdentityRole> roleManager;
        public readonly SignInManager<AppUser> signInManager;
        public readonly IPasswordHasher<AppUser> passwordHasher;
        public readonly IPasswordValidator<AppUser> passwordValidator;
        private IHostingEnvironment hostingEnvironment;
        private IFileNameGenerator nameGenerator;
        public readonly PayrollDbContext payrollDb;


        public AccountController(UserManager<AppUser> _userManager
                                   , RoleManager<IdentityRole> _roleManager
                                    , SignInManager<AppUser> _signInManager
                                      , IPasswordHasher<AppUser> _passwordHasher
                                       , IPasswordValidator<AppUser> _passwordValidator
                                        , PayrollDbContext _payrollDb
                                         , IHostingEnvironment _hostingEnvironment
                                           ,IFileNameGenerator _nameGenerator)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            signInManager = _signInManager;
            passwordHasher = _passwordHasher;
            passwordValidator = _passwordValidator;
            payrollDb = _payrollDb;
            hostingEnvironment = _hostingEnvironment;
            nameGenerator = _nameGenerator;
        }

        public IActionResult Index()
        {
            return View();
        }

        // account settings
        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            var data = HttpContext.Session.GetObjectFromJson<SessionUserModel>("UserData");
            var currentUser = payrollDb.Users.Where(x => x.Id == data.Id).FirstOrDefault();
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
        public async Task<ActionResult> Edit(AdminModel adminModel,IFormFile Image)
        {
          SessionUserModel sessionUser = HttpContext.Session.GetObjectFromJson<SessionUserModel>("UserData");
          AppUser user = userManager.Users.Where(x => x.Id == sessionUser.Id).First();
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
                                    SessionUserModel session = new SessionUserModel()
                                    {
                                        Id = user.Id,
                                        UserName = user.UserName,
                                        Email = user.Email,
                                        Photo = fileName,
                                        Role = str[0]
                                    };
                                    HttpContext.Session.SetObjectAsJson("UserData", session);
                                    payrollDb.Update<AppUser>(user);
                                    await payrollDb.SaveChangesAsync();
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
                                SessionUserModel session = new SessionUserModel()
                                {
                                    Id = user.Id,
                                    UserName = user.UserName,
                                    Email = user.Email,
                                    Photo = user.Photo,
                                    Role = str[0]
                                };
                                HttpContext.Session.SetObjectAsJson("UserData", session);
                                payrollDb.Update<AppUser>(user);
                                await payrollDb.SaveChangesAsync();
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
                            SessionUserModel session = new SessionUserModel()
                            {
                                Id = user.Id,
                                UserName = user.UserName,
                                Email = user.Email,
                                Photo = fileName,
                                Role = str[0]
                            };
                            HttpContext.Session.SetObjectAsJson("UserData", session);
                            payrollDb.Update<AppUser>(user);
                            await payrollDb.SaveChangesAsync();
                            ModelState.AddModelError("", "Succes");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Photo is not Format and format (jpg,png)");
                        }
                    }
                    else
                    {
                        SessionUserModel session = new SessionUserModel()
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            Photo = user.Photo,
                            Role = str[0]
                        };
                        HttpContext.Session.SetObjectAsJson("UserData", session);
                        payrollDb.Update<AppUser>(user);
                        await payrollDb.SaveChangesAsync();
                        ModelState.AddModelError("", "Succes");
                    }
                }
            }
            return View(adminModel);
        }
        [HttpGet]
        public async Task<ActionResult> Profil()
        {
           SessionUserModel sessionUser  =  HttpContext.Session.GetObjectFromJson<SessionUserModel>("UserData");
          var data =  await userManager.FindByIdAsync(sessionUser.Id);
            AdminModel admin = new AdminModel()
            {
                FirstName = data.FirstName,
                SecondName = data.SecondName,
                Image = data.Photo,
                Email = data.Email,
                Address = data.Adress,
                Birthday = data.Birth,
                PhoneNumber = data.PhoneNumber,
                Role = sessionUser.Role
            };
            return View(admin);
        }
        public async Task<ActionResult> LogOut()
        {
           await signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login","Account");
        }

        //add holding
        [HttpGet]
        public async Task<ActionResult> AddHolding()
        {
            var holding = await payrollDb.Holdings.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddHolding(Holding holding)
        {
            if(ModelState.IsValid)
            {
                    Holding holdin = new Holding
                    {
                        Name = holding.Name
                    };
                    await payrollDb.Holdings.AddAsync(holdin);
                    await payrollDb.SaveChangesAsync();
            }
            return View();
        }

        //add company
        [HttpGet]
        public async Task<ActionResult> AddCompany()
        {
            await Task.Delay(0);
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddCompany(Company company)
        {
           if(ModelState.IsValid)
            {
                var data = await payrollDb.Holdings.Where(x => x.Id == company.HoldingId).FirstOrDefaultAsync();
                if(data !=null)
                {
                    Company companys = new Company
                    {
                        Name = company.Name,
                        HoldingId = data.Id
                    };
                    await payrollDb.Companies.AddAsync(companys);
                    await payrollDb.SaveChangesAsync();
                    ModelState.AddModelError("", "Success");
                }
            }
            return View();
            
        }

        //add emporia
        [HttpGet]
        public async Task<ActionResult> AddEmporia()
        {
            await Task.Delay(0);
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddEmporia(Emporium emporium)
        {
            if(ModelState.IsValid)
            {
                var data = await payrollDb.Companies.Where(x => x.Id == emporium.CompanyId).FirstOrDefaultAsync();
                if(data != null)
                {
                    Emporium emporiums = new Emporium
                    {
                        Name = emporium.Name,
                        Address = emporium.Address,
                        CompanyId = data.Id
                    };
                    await payrollDb.Emporia.AddAsync(emporiums);
                    await payrollDb.SaveChangesAsync();
                }
                ModelState.AddModelError("", "Check Company Name");
            }
            return View();
        }

        //add position
        [HttpGet]
        public async Task<ActionResult> AddPosition()
        {
            await Task.Delay(0);
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddPosition(Positions positions)
        {
            if(ModelState.IsValid)
            {
                Positions position = new Positions
                {
                    Name = positions.Name
                };
                await payrollDb.Positions.AddAsync(position);
                await payrollDb.SaveChangesAsync();
            }
            return View();
        }

        //add departament
        [HttpGet]
        public async Task<ActionResult> AddDepartament()
        {
            await Task.Delay(0);
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddDepartament(Departament departament)
        {
           if(ModelState.IsValid)
            {
                var data = payrollDb.Holdings.Where(x => x.Id == departament.HoldingId).FirstOrDefaultAsync();
                if(data != null)
                {
                    Departament departaments = new Departament
                    {
                        Name = departament.Name,
                        HoldingId = data.Id
                    };
                    await payrollDb.Departaments.AddAsync(departaments);
                    await payrollDb.SaveChangesAsync();
                    ModelState.AddModelError("", "Success");
                }
                ModelState.AddModelError("", "Check holding name");
            }
            return View();
        }

        //Ajax Query
        public async Task<JsonResult> AddHolding(string value)
        {
            if(string.IsNullOrEmpty(value))
                return Json(new {message = 404 });
            Holding holdin = new Holding
            {
                Name = value
            };
            await payrollDb.Holdings.AddAsync(holdin);
            await payrollDb.SaveChangesAsync();
            return Json(new { message = 202, data = payrollDb.Holdings.ToList()});
        }
        [HttpPost]
        public async  Task<JsonResult> AjaxHolding()
        {
            var data = await payrollDb.Holdings.ToListAsync();
            return Json(new { company = data, message = 202 });
        }
        [HttpPost]
        public async Task<JsonResult> AjaxCompany()
        {
            List<Company> data =await payrollDb.Companies.ToListAsync();
            return Json(new { company = data, message = 202 });
        }




    }
}