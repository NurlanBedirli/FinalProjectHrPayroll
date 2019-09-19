using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrPayroll.Areas.Admin.Models;
using HrPayroll.Areas.Admin.PaginationModel;
using HrPayroll.Areas.Admin.MenecerAttandanceModel;
using HrPayroll.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HrPayroll.Areas.Admin.AttendanceBool;
using Microsoft.AspNetCore.Authorization;
using HrPayroll.Areas.Admin.BLL;
using HrPayroll.Areas.Admin.Options;
using Microsoft.Extensions.Options;
using HrPayroll.Areas.Admin.EmployeeModel;

namespace HrPayroll.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenecerController : Controller
    {
        public  PayrollDbContext dbContext { get; set; }
        public UserManager<AppUser> userManager { get; set; }
        public readonly IOptions<WorkIdOptions> options;

        public MenecerController(PayrollDbContext _dbContext, 
                                          UserManager<AppUser> _userManager,
                                               IOptions<WorkIdOptions> _options)
        {
            dbContext = _dbContext;
            userManager = _userManager;
            options = _options;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> EmployeeList()
        {
           List<Employee> employees = new List<Employee>();
           var menecerData = HttpContext.Session.GetObjectFromJson<AppUser>("UserData");
           var menecerByEmail =  await userManager.FindByEmailAsync(menecerData.Email);
           if(menecerByEmail !=null)
            {
               var data =  dbContext.EmporiumAppUsers.Where(x => x.AppUserId == menecerByEmail.Id).FirstOrDefault();
                if(data != null)
                {
                    var empwork = dbContext.Placeswork.Where(x => x.EmporiumId == data.EmporiumId).ToList();
                    foreach(var emp in empwork)
                    {
                        var employee = dbContext.Employees.Where(x => x.Id == emp.EmployeeId).FirstOrDefault();
                        employees.Add(employee);
                    }
                    int page = 1;
                    var elmpage = 5;
                    var pagingCount = Math.Ceiling(employees.Count / (decimal)elmpage);
                    Paging paging = new Paging
                    {
                        CurrentPage = page,
                        ItemPage = elmpage,
                        Prev = page>1,
                        Next = page<pagingCount,
                        TotalItems = employees.Count()
                    };
                    var allEmployee = employees.Skip((paging.CurrentPage - 1) * paging.ItemPage).Take(paging.ItemPage).ToList();
                    PagingModel model = new PagingModel()
                    {
                        Employees = allEmployee,
                        Paging = paging
                    };
                    return View(model);
                }
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task<ActionResult> MenecerEmployeeList(int? id,int pageSize)
        {
            List<Employee> employees = new List<Employee>();
            if(id == null)
            {
                id = 1;
            }
            var pageElm = Convert.ToInt32(pageSize);
            if(pageElm == 0)
            {
                pageElm = 5;
            }
            else
            {
                options.Value.PageSize = pageSize;// option select pagination Pagesize
            }

            var elmpage = 0;
            var menecerData = HttpContext.Session.GetObjectFromJson<SessionUserModel>("UserData");
            var menecerByEmail = await userManager.FindByEmailAsync(menecerData.Email);
            if (menecerByEmail != null)
            {
                var data = dbContext.EmporiumAppUsers.Where(x => x.AppUserId == menecerByEmail.Id).FirstOrDefault();
                if (data != null)
                {
                    var empwork = dbContext.Placeswork.Where(x => x.EmporiumId == data.EmporiumId).ToList();
                    foreach (var emp in empwork)
                    {
                        var employee = dbContext.Employees.Where(x => x.Id == emp.EmployeeId).FirstOrDefault();
                        employees.Add(employee);
                    }
                    int page = (int)id;
                   if(pageSize == 0)
                    {
                        if(options.Value.PageSize != 0)
                        {
                            elmpage = options.Value.PageSize;
                        }
                        else
                        {
                            elmpage = pageElm;
                        }
                    }
                   else
                    {
                        elmpage = options.Value.PageSize;
                    }
                    var pagingCount = Math.Ceiling(employees.Count / (decimal)elmpage);
                    Paging paging = new Paging
                    {
                        CurrentPage = page,
                        ItemPage = elmpage,
                        Prev = page > 1,
                        Next = page < pagingCount,
                        TotalItems = employees.Count()
                    };
                    var allEmployee = employees.Skip((paging.CurrentPage - 1) * paging.ItemPage).Take(paging.ItemPage).ToList();
                    PagingModel model = new PagingModel()
                    {
                        Employees = allEmployee,
                        Paging = paging
                    };
                    return View(model);
                }
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<ActionResult> WorkAttendance(int? id)
        {
            if(id != null)
            {
                var current = await dbContext.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();
                if(current != null)
                {
                    HttpContext.Session.SetObjectAsJson("Employ", current);
                    return View();
                }
            }
            return RedirectToAction("EmployeeList", "Menecer", new { area = "Admin" });
        }

        [HttpPost]
        public async Task<ActionResult> WorkAttendance(SignInOutReasonTbl model)
        {
            if(ModelState.IsValid)
            {
                var currentEmp = HttpContext.Session.GetObjectFromJson<Employee>("Employ");
                SignInOutReasonTbl signIn = new SignInOutReasonTbl
                {
                    SignInTime = model.SignInTime,
                    RaasonName = model.RaasonName,
                    PenaltyAmount = model.PenaltyAmount,
                    Status = model.Status,
                    EmployeeId = currentEmp.Id
                };
               await BoolAttendance.BoolSaveSignInOut(dbContext, model, signIn);
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Attandance()
        {
            List<List<EmployeeAttendance>> attendances = new List<List<EmployeeAttendance>>();
            List<EmployeeAttendance> No_note_attendance = new List<EmployeeAttendance>();

            var menecer =  HttpContext.Session.GetObjectFromJson<SessionUserModel>("UserData");
            if(menecer != null)
            {
                var MenecerEmporium = await dbContext.EmporiumAppUsers.Where(x => x.AppUserId == menecer.Id).FirstOrDefaultAsync();
                var Empworkplace = await dbContext.Placeswork.Where(x => x.EmporiumId == MenecerEmporium.EmporiumId).ToListAsync();
                foreach (var item in Empworkplace)
                {
                    var currentEmployee = await dbContext.Employees.Where(x => x.Id == item.EmployeeId).FirstOrDefaultAsync();
                    if (currentEmployee != null)
                    {
                        var a = dbContext.SignInOutReasons.Where(x => x.EmployeeId == item.EmployeeId).Select(z => new EmployeeAttendance
                        {
                            SignIn = z.SignIn,
                            AttandanceDate = z.SignInTime,
                            Name = currentEmployee.Name,
                            Surname = currentEmployee.Surname,
                            EmployeeId = z.EmployeeId
                        }).ToList();
                        if (a.Count != 0)
                        {
                            attendances.Add(a);
                        }
                        else
                        {
                            EmployeeAttendance employeeAttendance = new EmployeeAttendance
                            {
                                Name = currentEmployee.Name,
                                Surname = currentEmployee.Surname
                            };
                            No_note_attendance.Add(employeeAttendance);
                        }

                    }
                }
                var AbsentCount = await dbContext.AbsentCounts.ToListAsync();
                var discipline = await dbContext.DisciplinePenalties.FirstOrDefaultAsync();
                AttandanceTable attandanceTable = new AttandanceTable
                {
                    attendances = attendances,
                    employeeAttendances = No_note_attendance,
                    AbsentCount = AbsentCount,
                    DisciplinePenalty = discipline
                };
                return View(attandanceTable);
            }
            return RedirectToAction("Login", "Account");
           
        }

        [HttpGet]
        public async Task<ActionResult> PenltyAttandance(int? id,int? count)
        {
             if(id != null)
            {
               var discipline =  await dbContext.DisciplinePenalties.FirstOrDefaultAsync();
                if(discipline != null)
                {
                    if(PenaltyCalculate.CountIsBigNumberMin((int)count,discipline.MinDay))
                    {
                        var emp = await dbContext.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();
                        if (emp != null)
                        {
                            HttpContext.Session.SetObjectAsJson("Employ", emp);
                            HttpContext.Session.SetObjectAsJson("AbsentCount", count);
                            return View();
                        }
                        return RedirectToAction("Attandance", "Menecer", new { area = "Admin" });
                    }
                     ModelState.AddModelError("aa", "Please follow the discipline");
                    
                }
                ModelState.AddModelError("", "Report the problem to the holding");
            }
            return RedirectToAction("Attandance", "Menecer", new { area = "Admin" });
        }

        [HttpPost]
        public async Task<ActionResult> PenltyAttandance(AbsentCount absent)
        {
            if(ModelState.IsValid)
            {
                var empdata = HttpContext.Session.GetObjectFromJson<Employee>("Employ");
                var count = HttpContext.Session.GetObjectFromJson<int>("AbsentCount");
                var absentCount = await dbContext.AbsentCounts.Where(x => x.EmployeeId == empdata.Id).FirstOrDefaultAsync();
                if(absentCount != null)
                {
                    dbContext.AbsentCounts.Remove(absentCount);
                    AbsentCount countAbsent = new AbsentCount
                    {
                        Count = count,
                        DateTime = absent.DateTime.Date.Date,
                        EmployeeId = empdata.Id
                    };
                    dbContext.AbsentCounts.Remove(absentCount);
                    dbContext.AbsentCounts.Add(countAbsent);
                    await dbContext.SaveChangesAsync();
                    await PenaltyCalculate.EmployeePenaltySum(dbContext, count, empdata.Id,absent.DateTime);
                }
                else
                {
                    AbsentCount countAbsent = new AbsentCount
                    {
                        Count = count,
                        DateTime = absent.DateTime,
                        EmployeeId = empdata.Id
                    };
                    dbContext.AbsentCounts.Add(countAbsent);
                    await dbContext.SaveChangesAsync();
                    await PenaltyCalculate.EmployeePenaltySum(dbContext, count, empdata.Id,absent.DateTime);
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> BonusEmployee(int? id)
        {
            if(id != null)
            {
                var data = await dbContext.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();
                if(data != null)
                {
                    var bonus = await dbContext.Bonus.Where(c => c.EmployeeId == data.Id).FirstOrDefaultAsync();
                    if(bonus == null)
                    {
                        HttpContext.Session.SetObjectAsJson("Employ", data);
                        return View();
                    }
                }
            }

            return RedirectToAction("MenecerEmployeeList", "Menecer",new { area = "Admin"});
        }
       
        [HttpPost]
        public async Task<ActionResult> BonusEmployee(Bonus bonus)
        {
            if(ModelState.IsValid)
            {
                var menecer = HttpContext.Session.GetObjectFromJson<SessionUserModel>("UserData");
                var data =  HttpContext.Session.GetObjectFromJson<Employee>("Employ");
                if(data != null)
                {
                    var bonusData = await dbContext.Bonus.Where(x => x.EmployeeId == data.Id).FirstOrDefaultAsync();
                    if(bonusData == null)
                    {
                    using (var transaction = await dbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            Bonus bonuses = new Bonus
                            {
                                BonusDate = bonus.BonusDate,
                                BonusPrize = bonus.BonusPrize,
                                BonusStatus = bonus.BonusStatus,
                                EmployeeId = data.Id,
                                AppUserId = menecer.Id
                            };
                            await dbContext.Bonus.AddAsync(bonuses);
                            await dbContext.SaveChangesAsync();
                            transaction.Commit();
                            ModelState.AddModelError("", "Success");
                            return View();
                        }
                        catch(Exception ex)
                        {
                            ModelState.AddModelError("", ex.Message);
                        }
                    }

                    }
                    ModelState.AddModelError("", "Such an employee already has a bonus");
                }
            }
            return RedirectToAction("EmployeePositionTable","Menecer",new {area = "Admin"});
        }

        [HttpGet]
        public async Task<ActionResult> EmployeePositionTable()
        {
            List<WorkPlace> workPlaces = new List<WorkPlace>();
            List<MenecerEmployeeBonusModel> employeeBonusModels = new List<MenecerEmployeeBonusModel>();
            try
            {
                var menecer = HttpContext.Session.GetObjectFromJson<SessionUserModel>("UserData");
                if (menecer != null)
                {
                    var bonuses = await dbContext.Bonus.Where(x => x.AppUserId == menecer.Id).ToListAsync();
                    if (bonuses != null)
                    {
                        foreach(var emp in bonuses)
                        {
                          var EmployeeWork =  await dbContext.Placeswork.Where(x => x.EmployeeId == emp.EmployeeId).FirstOrDefaultAsync();
                            workPlaces.Add(EmployeeWork);
                        }
                        foreach(var place in workPlaces)
                        {
                            MenecerEmployeeBonusModel bonusModel = new MenecerEmployeeBonusModel
                            {
                                Emporium = await dbContext.Emporia.Where(x => x.Id == place.EmporiumId).Select(y => y.Name).FirstOrDefaultAsync(),
                                Position = await dbContext.Positions.Where(x => x.Id == place.PositionsId).Select(y => y.Name).FirstOrDefaultAsync(),
                                Name = await dbContext.Employees.Where(x => x.Id == place.EmployeeId).Select(y => y.Name).FirstOrDefaultAsync(),
                                Bonus = bonuses.Where(x => x.EmployeeId == place.EmployeeId).Select(y => y.BonusPrize).FirstOrDefault(),
                                StartDate = place.StarDate,
                                EmployeeId = place.EmployeeId
                            };
                            employeeBonusModels.Add(bonusModel);
                        }
                        return View(employeeBonusModels);
                    }
                }
            }
            catch(Exception exp)
            {
                ModelState.AddModelError("", exp.Message);
            }
            return RedirectToAction(nameof(MenecerEmployeeList));
        }

        [HttpPost]
        public async Task<ActionResult> BonusEdit(int? id)
        {
            if(id !=null)
            {
               var Bonusdata =  await dbContext.Bonus.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();

                if(Bonusdata != null)
                {
                    return View(Bonusdata);
                }
            }
            return View();
        }

        //[HttpPost]
        //public async Task<JsonResult> PagingMenecment(string count, int elmPage)
        //{
        //    List<Employee> employees = new List<Employee>();
        //    var menecerData = HttpContext.Session.GetObjectFromJson<AppUser>("UserData");
        //    var menecerByEmail = await userManager.FindByEmailAsync(menecerData.Email);
        //    if (menecerByEmail != null)
        //    {
        //        var dataa = dbContext.EmporiumAppUsers.Where(x => x.AppUserId == menecerByEmail.Id).FirstOrDefault();
        //        if (dataa != null)
        //        {
        //            var empwork = dbContext.Placeswork.Where(x => x.EmporiumId == dataa.EmporiumId).ToList();
        //            foreach (var emp in empwork)
        //            {
        //                var employee = dbContext.Employees.Where(x => x.Id == emp.EmployeeId).FirstOrDefault();
        //                employees.Add(employee);
        //            }
        //        }
        //    }

        //    int? page = Convert.ToInt32(count);
        //    if (page == null)
        //    {
        //        page = 1;
        //    }
        //    if (elmPage == 0)
        //    {
        //        elmPage = 10;
        //    }
        //    int elmpage = elmPage;
        //    var pageCount = Math.Ceiling(employees.Count() / (decimal)elmpage);/* 150/15=10*/
        //    Paging paging1 = new Paging
        //    {
        //        CurrentPage = (int)page,
        //        ItemPage = elmpage,
        //        TotalItems = employees.Count(),
        //        Prev = page > 1,
        //        Next = page < pageCount
        //    };

        //    var data = employees.Skip((paging1.CurrentPage - 1) * paging1.ItemPage).Take(paging1.ItemPage).ToList();

        //    return Json(new { pageCount = pageCount, currentPage = paging1.CurrentPage, nextElement = paging1.Next, prevElement = paging1.Prev, currentData = data, message = 202 });
        //}

        //public async Task<JsonResult> SearchWorkPlace(string value, decimal salary, int elmPage)
        //{
        //    int? page = 1;

        //    if (elmPage == 0)
        //    {
        //        elmPage = 10;
        //    }
        //    int elmpage = elmPage;
        //    decimal pageCount = 0;
        //    Paging paging = new Paging();
        //    paging.CurrentPage = (int)page;
        //    paging.ItemPage = elmpage;
        //    paging.Prev = page > 1;

        //    List<Employee> employees = new List<Employee>();
        //    var menecerData = HttpContext.Session.GetObjectFromJson<AppUser>("UserData");
        //    var menecerByEmail = await userManager.FindByEmailAsync(menecerData.Email);
        //    if (menecerByEmail != null)
        //    {
        //        var dataa = dbContext.EmporiumAppUsers.Where(x => x.AppUserId == menecerByEmail.Id).FirstOrDefault();
        //        if (dataa != null)
        //        {
        //            var empwork = dbContext.Placeswork.Where(x => x.EmporiumId == dataa.EmporiumId).ToList();
        //            foreach (var emp in empwork)
        //            {
        //                var employee = dbContext.Employees.Where(x => x.Id == emp.EmployeeId).FirstOrDefault();
        //                employees.Add(employee);
        //            }
        //        }
        //    }

        //    if (value != null)
        //    {

        //        var searchData = employees.Where(x => x.Name.Contains(value) || x.Name.StartsWith(value) ||
        //                   x.Surname.Contains(value) || x.Surname.StartsWith(value)
        //                             || x.Email.Contains(value) || x.Email.StartsWith(value)
        //                                || x.PlasiyerCode.Contains(value) || x.PlasiyerCode.StartsWith(value)
        //                                   || x.Number.Contains(value) || x.Number.StartsWith(value)
        //                                      || x.Nationally.Contains(value)).ToList();

        //        paging.TotalItems = searchData.Count();
        //        pageCount = Math.Ceiling(searchData.Count() / (decimal)elmpage);
        //        paging.Next = page < pageCount;

        //        var datapaging = searchData.Skip((paging.CurrentPage - 1) * elmpage).Take(elmpage).ToList();
        //        return Json(new { pageCount = pageCount, currentPage = paging.CurrentPage, allData = searchData, nextElement = paging.Next, prevElement = paging.Prev, data = datapaging, message = 202 });
        //    }


        //    paging.TotalItems = employees.Count();
        //    pageCount = Math.Ceiling(employees.Count() / (decimal)elmpage);
        //    paging.Next = page < pageCount;
        //    var data = employees.Skip((paging.CurrentPage - 1) * elmpage).Take(elmpage).ToList();
        //    return Json(new { pageCount = pageCount, currentPage = paging.CurrentPage, allData = employees, nextElement = paging.Next, prevElement = paging.Prev, data = data, message = 202 });
        //}

    }
}