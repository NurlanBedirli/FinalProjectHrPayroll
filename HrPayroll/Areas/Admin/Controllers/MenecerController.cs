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

namespace HrPayroll.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenecerController : Controller
    {
        public  PayrollDbContext dbContext { get; set; }
        public UserManager<AppUser> userManager { get; set; }
        public MenecerController(PayrollDbContext _dbContext, UserManager<AppUser> _userManager)
        {
            dbContext = _dbContext;
            userManager = _userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

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
            var MenecerEmporium = await dbContext.EmporiumAppUsers.Where(x => x.AppUserId == menecer.Id).FirstOrDefaultAsync();
            var Empworkplace = await dbContext.Placeswork.Where(x => x.EmporiumId == MenecerEmporium.EmporiumId).ToListAsync();
            foreach(var item in Empworkplace)
            {
               var currentEmployee = await dbContext.Employees.Where(x => x.Id == item.EmployeeId).FirstOrDefaultAsync();
                if(currentEmployee != null)
                {
                    var a = dbContext.SignInOutReasons.Where(x => x.EmployeeId == item.EmployeeId).Select(z => new EmployeeAttendance
                    {
                        SignIn = z.SignIn,
                        AttandanceDate = z.SignInTime,
                        Name = currentEmployee.Name,
                        Surname = currentEmployee.Surname,
                        EmployeeId = z.EmployeeId
                    }).ToList();
                    if(a.Count != 0)
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
            AttandanceTable attandanceTable = new AttandanceTable
            {
                attendances = attendances,
                employeeAttendances = No_note_attendance,
                AbsentCount = AbsentCount
            };
            return View(attandanceTable);
        }

        [HttpGet]
        public async Task<ActionResult> PenltyAttandance(int? id,int? count)
        {
             if(id != null)
            {
               var discipline =  await dbContext.DisciplinePenalties.FirstOrDefaultAsync();
                if(discipline != null)
                {
                    if(PenaltyCalculate.CountIsBigNumber((int)count, discipline.Name))
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
                        DateTime = absent.DateTime
                    };
                    dbContext.AbsentCounts.Remove(absentCount);
                    dbContext.AbsentCounts.Add(countAbsent);
                    await dbContext.SaveChangesAsync();
                    await PenaltyCalculate.EmployeePenaltySum(dbContext, count, empdata.Id);
                }
                else
                {
                   await PenaltyCalculate.EmployeePenaltySum(dbContext, count, empdata.Id);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> PagingMenecmentAjax(string count, int elmPage)
        {
            List<Employee> employees = new List<Employee>();
            var menecerData = HttpContext.Session.GetObjectFromJson<AppUser>("UserData");
            var menecerByEmail = await userManager.FindByEmailAsync(menecerData.Email);
            if (menecerByEmail != null)
            {
                var dataa = dbContext.EmporiumAppUsers.Where(x => x.AppUserId == menecerByEmail.Id).FirstOrDefault();
                if (dataa != null)
                {
                    var empwork = dbContext.Placeswork.Where(x => x.EmporiumId == dataa.EmporiumId).ToList();
                    foreach (var emp in empwork)
                    {
                        var employee = dbContext.Employees.Where(x => x.Id == emp.EmployeeId).FirstOrDefault();
                        employees.Add(employee);
                    }
                }
            }

            int? page = Convert.ToInt32(count);
            if (page == null)
            {
                page = 1;
            }
            if (elmPage == 0)
            {
                elmPage = 10;
            }
            int elmpage = elmPage;
            var currentData = employees;
            var pageCount = Math.Ceiling(employees.Count() / (decimal)elmpage);/* 150/15=10*/
            Paging paging1 = new Paging
            {
                CurrentPage = (int)page,
                ItemPage = elmpage,
                TotalItems = currentData.Count(),
                Prev = page > 1,
                Next = page < pageCount
            };

            var data = employees.Skip((paging1.CurrentPage - 1) * paging1.ItemPage).Take(paging1.ItemPage).ToList();

            return Json(new { pageCount = pageCount, currentPage = paging1.CurrentPage, nextElement = paging1.Next, prevElement = paging1.Prev, currentData = data, message = 202 });
        }

        public async Task<JsonResult> SearchWorkPlace(string value, decimal salary, int elmPage)
        {
            int? page = 1;

            if (elmPage == 0)
            {
                elmPage = 10;
            }
            int elmpage = elmPage;
            decimal pageCount = 0;
            Paging paging = new Paging();
            paging.CurrentPage = (int)page;
            paging.ItemPage = elmpage;
            paging.Prev = page > 1;

            List<Employee> employees = new List<Employee>();
            var menecerData = HttpContext.Session.GetObjectFromJson<AppUser>("UserData");
            var menecerByEmail = await userManager.FindByEmailAsync(menecerData.Email);
            if (menecerByEmail != null)
            {
                var dataa = dbContext.EmporiumAppUsers.Where(x => x.AppUserId == menecerByEmail.Id).FirstOrDefault();
                if (dataa != null)
                {
                    var empwork = dbContext.Placeswork.Where(x => x.EmporiumId == dataa.EmporiumId).ToList();
                    foreach (var emp in empwork)
                    {
                        var employee = dbContext.Employees.Where(x => x.Id == emp.EmployeeId).FirstOrDefault();
                        employees.Add(employee);
                    }
                }
            }

            if (value != null)
            {

                var searchData = employees.Where(x => x.Name.Contains(value) || x.Name.StartsWith(value) ||
                           x.Surname.Contains(value) || x.Surname.StartsWith(value)
                                     || x.Email.Contains(value) || x.Email.StartsWith(value)
                                        || x.PlasiyerCode.Contains(value) || x.PlasiyerCode.StartsWith(value)
                                           || x.Number.Contains(value) || x.Number.StartsWith(value)
                                              || x.Nationally.Contains(value)).ToList();

                paging.TotalItems = searchData.Count();
                pageCount = Math.Ceiling(searchData.Count() / (decimal)elmpage);
                paging.Next = page < pageCount;

                var datapaging = searchData.Skip((paging.CurrentPage - 1) * elmpage).Take(elmpage).ToList();
                return Json(new { pageCount = pageCount, currentPage = paging.CurrentPage, allData = searchData, nextElement = paging.Next, prevElement = paging.Prev, data = datapaging, message = 202 });
            }


            paging.TotalItems = employees.Count();
            pageCount = Math.Ceiling(employees.Count() / (decimal)elmpage);
            paging.Next = page < pageCount;
            var data = employees.Skip((paging.CurrentPage - 1) * elmpage).Take(elmpage).ToList();
            return Json(new { pageCount = pageCount, currentPage = paging.CurrentPage, allData = employees, nextElement = paging.Next, prevElement = paging.Prev, data = data, message = 202 });
        }

    }
}