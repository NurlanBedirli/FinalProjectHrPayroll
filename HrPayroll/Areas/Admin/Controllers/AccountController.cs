using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HrPayroll.Areas.Admin.AddHolEmPosDepModel;
using HrPayroll.Areas.Admin.AddHolEmpPosDepModel;
using HrPayroll.Areas.Admin.EmployeeModel;
using HrPayroll.Areas.Admin.Models;
using HrPayroll.Areas.Admin.PaginationModel;
using HrPayroll.Areas.Admin.ViewModel;
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
        public async Task<ActionResult> Edit(string id)
        {
           
                var data = HttpContext.Session.GetObjectFromJson<SessionUserModel>("UserData");
               AppUser  currentUser = payrollDb.Users.Where(x => x.Id == data.Id).FirstOrDefault();
            
            
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
        [HttpGet]
        public async Task<ActionResult> LogOut()
        {
           await signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login","Account");
        }
        [HttpGet]
        public async Task<ActionResult> AddMonthSalePrize()
        {
            ViewBag.Holding = await payrollDb.Holdings.ToListAsync();

            ViewBag.MonthSale  =  payrollDb.MonthSales
                        .Include(x => x.Emporium)
                                        .ThenInclude(y => y.Company)
                                                    .ThenInclude(a => a.Holding)
                                                        .Select(b => new MonthSaleModel
                                                        {
                                                            Company = b.Emporium.Company.Name,
                                                            Emporium = b.Emporium.Name,
                                                            Holding = b.Emporium.Company.Holding.Name,
                                                            Prize = b.Prize,
                                                            Id = b.Id
                                                        }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddMonthSalePrize(MonthSale month)
        {
            if(ModelState.IsValid)
            {
               try
                {
                    if (month.EmporiumId !=0)
                    {
                    var data =  payrollDb.Emporia.Where(x => x.Id == month.EmporiumId).FirstOrDefault();
                        if(data != null)
                        {
                            bool found = payrollDb.MonthSales.Where(y => y.EmporiumId == data.Id)
                                              .Any(x => x.DateQuata.Month == month.DateQuata.Month);
                            if (!found)
                            {
                                    await payrollDb.MonthSales.AddAsync(month);
                                    await payrollDb.SaveChangesAsync();
                                    ModelState.AddModelError("", "Success");
                            }
                            else
                            {
                                ModelState.AddModelError("", "");
                            }
                        }
                    }
                }
                catch(Exception exp)
                {
                    ModelState.AddModelError("", exp.Message);
                }
            }
            ViewBag.Holding = await payrollDb.Holdings.ToListAsync();
            ViewBag.MonthSale = payrollDb.MonthSales.Include(x => x.Emporium).ThenInclude(y => y.Company).ThenInclude(a => a.Holding)
                                                            .Select(b => new MonthSaleModel
                                                            {
                                                                Company = b.Emporium.Company.Name,
                                                                Emporium = b.Emporium.Name,
                                                                Holding = b.Emporium.Company.Holding.Name,
                                                                Prize = b.Prize,
                                                                Quata = b.Quata,
                                                                Date = b.DateQuata,
                                                                Id = b.Id
                                                            }).ToList();
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> MonthSaleDel(int? id)
        {
            if(id.HasValue)
            {
                var data = payrollDb.MonthSales.Where(x => x.Id == id).FirstOrDefault();
                if(data != null)
                {
                    payrollDb.MonthSales.Remove(data);
                    await payrollDb.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(AddMonthSalePrize));
        }

        public ActionResult Calendar()
        {
            return View();
        }

        //add holding
        [HttpGet]
        public async Task<ActionResult> AddHolding()
        {
            ViewBag.Holding = await payrollDb.Holdings.ToListAsync();
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
                ViewBag.Holding = await payrollDb.Holdings.ToListAsync();
            }
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> DeleteHolding(int? id)
        {
            if (!id.HasValue)
                return NotFound();
            var holding = payrollDb.Holdings.Where(x => x.Id == id).FirstOrDefault();
            if(holding != null)
            {
                payrollDb.Holdings.Remove(holding);
                await payrollDb.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AddHolding));
        }


        //add company
        [HttpGet]
        public async Task<ActionResult> AddCompany()
        {
            ViewBag.Holding =  payrollDb.Holdings.Include("Company").Select(x => new Holding
            {
                Name = x.Name,
                Companies = x.Companies,
                Id = x.Id
            }).ToList();
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
            ViewBag.Holding = payrollDb.Holdings.Include("Company").Select(x => new Holding
            {
                Name = x.Name,
                Companies = x.Companies,
                Id = x.Id
            }).ToList();
            return View();
            
        }

        [HttpGet]
        public async Task<ActionResult> DeleteCompany(int? id)
        {
            if(id.HasValue)
            {
               var data =  payrollDb.Companies.Where(x => x.Id == id).FirstOrDefault();
                if(data != null)
                {
                    payrollDb.Companies.Remove(data);
                    await payrollDb.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(AddCompany));
        }

        //add emporia
        [HttpGet]
        public async Task<ActionResult> AddEmporia()
        {
            ViewBag.Emporia = payrollDb.Emporia
                                     .Include("Company")
                                                 .Select(x => new Emporium
            {
                 Address = x.Address,
                 Company = x.Company,
                 Id = x.Id,
                 Name = x.Name,
            }).ToList();
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
                    ModelState.AddModelError("", "Success");
                    ViewBag.Emporia = payrollDb.Emporia
                                  .Include("Company")
                                              .Select(x => new Emporium
                                              {
                                                  Address = x.Address,
                                                  Company = x.Company,
                                                  Id = x.Id,
                                                  Name = x.Name,
                                              }).ToList();
                    return View();
                }
                ModelState.AddModelError("", "Check Company Name");
            }
            ViewBag.Emporia = payrollDb.Emporia
                                   .Include("Company")
                                               .Select(x => new Emporium
                                               {
                                                   Address = x.Address,
                                                   Company = x.Company,
                                                   Id = x.Id,
                                                   Name = x.Name,
                                               }).ToList();
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> DeleteEmporia(int? id)
        {
            if (id.HasValue)
            {
                var data = payrollDb.Emporia.Where(x => x.Id == id).FirstOrDefault();
                if (data != null)
                {
                    payrollDb.Emporia.Remove(data);
                    await payrollDb.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(AddEmporia));
        }

        
        //Baxilacaq Yoxlamaq lazimdi
        [HttpGet]
        public  ActionResult AddPosition()
        {
            ViewBag.Data = payrollDb.PositionsDepartaments
                .Include(x => x.Positions)
                     .ThenInclude(y => y.EmployeeSalaries)
                        .Include(a => a.Departament)
                           .Select(c => new WorkPlaceModel
                           {
                               DepartamentName = c.Departament.Name,
                               PositionName = c.Positions.Name,
                               Salary = c.Positions.EmployeeSalaries.Salary,
                               PositionId = c.Positions.Id
                           }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddPosition(AddCategoryModel  workPlace)
        {
            try
            {

                if(workPlace.DepartamentId != 0)
                {
                    int? Id = Convert.ToInt32(workPlace.DepartamentId);
                    if (Id.HasValue)
                    {
                        Positions positions = new Positions
                        {
                            Name = workPlace.PositionName,
                        };
                        await payrollDb.Positions.AddAsync(positions);
                        await payrollDb.SaveChangesAsync();

                        EmployeeSalary employeeSalary = new EmployeeSalary
                        {
                            Salary = workPlace.Salary,
                            PositionsId = positions.Id
                        };
                        await payrollDb.EmployeeSalaries.AddAsync(employeeSalary);
                        await payrollDb.SaveChangesAsync();

                        PositionsDepartament positionsDepartament = new PositionsDepartament
                        {
                            PositionsId = positions.Id,
                            DepartamentId = (int)Id
                        };
                        await payrollDb.PositionsDepartaments.AddAsync(positionsDepartament);
                        await payrollDb.SaveChangesAsync();
                    }
                }else
                {
                        Positions positions = new Positions
                        {
                            Name = workPlace.PositionName,
                        };
                        await payrollDb.Positions.AddAsync(positions);
                        await payrollDb.SaveChangesAsync();

                        EmployeeSalary employeeSalary = new EmployeeSalary
                        {
                            Salary = workPlace.Salary,
                            PositionsId = positions.Id
                        };
                        await payrollDb.EmployeeSalaries.AddAsync(employeeSalary);
                        await payrollDb.SaveChangesAsync();
                }
              
            }
            catch(Exception exp)
            {
                ModelState.AddModelError("", exp.Message);
            }


            return RedirectToAction(nameof(AddPosition));
        }

        [HttpGet]
        public async Task<ActionResult> DeletePositions(int? id)
        {
            if (id.HasValue)
            {
                var positionsData = payrollDb.Positions.Where(x => x.Id == id).FirstOrDefault();
                var EmporiumPosition = await payrollDb.EmporiumPositions.Where(x => x.PositionsId == id).FirstOrDefaultAsync();
                var departamentPosition = await payrollDb.PositionsDepartaments.Where(x => x.PositionsId == id).FirstOrDefaultAsync();
                var salary =  payrollDb.EmployeeSalaries.Where(x => x.PositionsId == id).FirstOrDefault();
                if (positionsData != null)
                {
                    payrollDb.Positions.Remove(positionsData);
                }
                if (EmporiumPosition != null)
                {
                    payrollDb.EmporiumPositions.Remove(EmporiumPosition);
                }
                if (salary != null)
                {
                    payrollDb.EmployeeSalaries.Remove(salary);
                  
                }
                if (departamentPosition != null)
                {
                    payrollDb.PositionsDepartaments.Remove(departamentPosition);

                }
                await payrollDb.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AddPosition));
        }


        //Position Emporium Foreign key
        [HttpGet]
        public ActionResult PositionEmporium()
        {
            ViewBag.data = payrollDb.EmporiumPositions
                          .Include(x => x.Emporium)
                                   .ThenInclude(y => y.Company)
                                           .Include(d => d.Positions.PositionsDepartaments.Departament)
                                                     .Include(a => a.Positions)
                                                         .ThenInclude(b => b.EmployeeSalaries)
                                                                   .Select(c => new WorkPlaceModel
                                                                   {
                                                                       CompanyName = c.Emporium.Company.Name,
                                                                       EmporiaName = c.Emporium.Name,
                                                                       PositionName = c.Positions.Name,
                                                                       Salary = c.Positions.EmployeeSalaries.Salary,
                                                                       PositionId = c.Positions.EmployeeSalaries.PositionsId,
                                                                       DepartamentName = c.Positions.PositionsDepartaments.Departament.Name,
                                                                       EmpPosId = c.Id
                                                                   }).ToList();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> PositionEmporium(AddCategoryModel workPlace)
        {
            try
            {
                int? Id = Convert.ToInt32(workPlace.EmporiaName);
                int? PositionId = Convert.ToInt32(workPlace.PositionName);
                if (Id.HasValue && PositionId.HasValue)
                {
                  bool isfound = payrollDb.EmporiumPositions
                            .Any(x => x.EmporiumId == Id && x.PositionsId == PositionId);

                    if(!isfound)
                    {
                        EmporiumPosition emporiumPosition = new EmporiumPosition
                        {
                            EmporiumId = (int)Id,
                            PositionsId = (int)PositionId
                        };
                        await payrollDb.EmporiumPositions.AddAsync(emporiumPosition);
                        await payrollDb.SaveChangesAsync();
                    }
                }
            }
            catch (Exception exp)
            {
                ModelState.AddModelError("", exp.Message);
            }


            return RedirectToAction(nameof(PositionEmporium));
        }
        [HttpGet]
        public async  Task<ActionResult> DeletePositionsEmperium(int? id)
        {
            if (id.HasValue)
            {
                var EmporiumPosition = await payrollDb.EmporiumPositions.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (EmporiumPosition != null)
                {
                    payrollDb.EmporiumPositions.Remove(EmporiumPosition);
                }
                await payrollDb.SaveChangesAsync();
            }
            return RedirectToAction(nameof(PositionEmporium));
        }


        //add departament
        [HttpGet]
        public ActionResult AddDepartament()
        {
            ViewBag.Data = payrollDb.Departaments
                     .Include(x => x.Holding)
                                  .Select(c => new AddCategoryModel
            {
                DepartamentName = c.Name,
                DepartamentId = c.Id,
                HoldingName = c.Holding.Name
            }).ToList();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddDepartament(Departament departament)
        {
           if(ModelState.IsValid)
            {
                var data =await payrollDb.Holdings.Where(x => x.Id == departament.HoldingId).FirstOrDefaultAsync();
                if(data != null)
                {
                    Departament departaments = new Departament
                    {
                        Name = departament.Name,
                        HoldingId = departament.HoldingId
                    };
                    await payrollDb.Departaments.AddAsync(departaments);
                    await payrollDb.SaveChangesAsync();
                    ModelState.AddModelError("", "Success");
                }
                ModelState.AddModelError("", "Check holding name");
            }
            return RedirectToAction(nameof(AddDepartament));
        }
        [HttpGet]
        public async Task<ActionResult> DeleteDepartment(int? id)
        {
            if(id.HasValue)
            {
                var data = payrollDb.Departaments.Where(x => x.Id == id).FirstOrDefault();
                if(data != null)
                {
                    payrollDb.Departaments.Remove(data);
                    await payrollDb.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(AddDepartament));
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

        [HttpPost]
        public async Task<JsonResult> AjaxDepartament()
        {
            var data = await payrollDb.Departaments.ToListAsync();
            return Json(new { company = data, message = 200 });
        }

        [HttpPost]
        public async Task<JsonResult> AjaxPosition()
        {
            var data = await payrollDb.Positions.ToListAsync();
            return Json(new { company = data, message = 200 });
        }
    }
}