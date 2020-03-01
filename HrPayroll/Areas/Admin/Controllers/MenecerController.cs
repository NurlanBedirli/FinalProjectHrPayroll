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
using HrPayroll.Areas.Admin.ViewModel;

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

        [HttpGet]
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
                var data = dbContext.EmporiumAppUsers
                                   .Where(x => x.AppUserId == menecerByEmail.Id)
                                           .FirstOrDefault();
                if (data != null)
                {
                    var empwork = dbContext.Placeswork
                                    .Where(x => x.EmporiumId == data.EmporiumId)
                                           .ToList();

                    foreach (var emp in empwork)
                    {
                        var employee = dbContext.Employees
                                          .Where(x => x.Id == emp.EmployeeId)
                                                .FirstOrDefault();

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

                bool foundAttandance = dbContext.SignInOutReasons
                                    .Where(x => x.EmployeeId == currentEmp.Id)
                                          .Any(x => x.SignInTime == model.SignInTime);
                if (!foundAttandance)
                {
                    //hans tarixde ise baslayib 1 cixiriq ki,hemin tarixi cedvele qeyd ede bilsin
                   bool fountStartWorkDate =  dbContext.Placeswork.Where(x => x.EmployeeId == currentEmp.Id)
                                   .Any(x => x.StarDate.Day-1 < model.SignInTime.Day);

                    if(fountStartWorkDate)
                    {

                        SignInOutReasonTbl signIn = new SignInOutReasonTbl
                        {
                            SignInTime = model.SignInTime,
                            RaasonName = model.RaasonName,
                            PenaltyAmount = model.PenaltyAmount,
                            Status = model.Status,
                            EmployeeId = currentEmp.Id
                        };
                        await BoolAttendance.BoolSaveSignInOut(dbContext, model, signIn);
                        ModelState.AddModelError("", "Success");
                        return View();
                    }
                    ModelState.AddModelError("", "There was no such employee in our company at that date");
                    return View();
                }
                ModelState.AddModelError("", "This date has already been added");
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Attandance(DateTime time)
        {
            try
            {
                if(time.Year == 1)
                {
                    time = DateTime.Now;
                }

                List<List<EmployeeAttendance>> attendances = new List<List<EmployeeAttendance>>();
                List<EmployeeAttendance> No_note_attendance = new List<EmployeeAttendance>();

                var menecer = HttpContext.Session.GetObjectFromJson<SessionUserModel>("UserData");
                if (menecer != null)
                {

                    var MenecerEmporium = await dbContext.EmporiumAppUsers
                                                  .Where(x => x.AppUserId == menecer.Id)
                                                    .FirstOrDefaultAsync();

                    var Empworkplace = await dbContext.Placeswork
                                                  .Where(x => x.EmporiumId == MenecerEmporium.EmporiumId)
                                                    .ToListAsync();


                    foreach (var item in Empworkplace)
                    {
                        var currentEmployee = await dbContext.Placeswork
                                                   .Where(x => x.EmployeeId == item.EmployeeId)
                                                      .Include(x=> x.Employee)
                                                        .FirstOrDefaultAsync();

                        if (currentEmployee != null)
                        {
                            var a = dbContext.SignInOutReasons.Where(x => x.EmployeeId == item.EmployeeId && x.SignInTime.Month == time.Month)
                                .Select(z => new EmployeeAttendance
                            {
                                SignIn = z.SignIn,
                                AttandanceDate = z.SignInTime,
                                Name = currentEmployee.Employee.Name,
                                Surname = currentEmployee.Employee.Surname,
                                EmployeeId = z.EmployeeId,
                                EmpStartDate = currentEmployee.StarDate,
                                ReasonName = z.RaasonName
                            }).ToList();
                            if (a.Count != 0)
                            {
                                attendances.Add(a);
                            }
                            else
                            {
                                EmployeeAttendance employeeAttendance = new EmployeeAttendance
                                {
                                    Name = currentEmployee.Employee.Name,
                                    Surname = currentEmployee.Employee.Surname
                                };
                                No_note_attendance.Add(employeeAttendance);
                            }

                        }
                    }
                    var AbsentCount = await dbContext.AbsentCounts.ToListAsync();
                    var discipline = await dbContext.DisciplinePenalties.FirstOrDefaultAsync();
                    var vacation = await dbContext.VacationEmployees
                             .Where(x=> x.StarDate.Month == time.Month || x.EndDate.Month == time.Month )
                                .ToListAsync();

                    AttandanceTable attandanceTable = new AttandanceTable
                    {
                        attendances = attendances,
                        employeeAttendances = No_note_attendance,
                        AbsentCount = AbsentCount,
                        DisciplinePenalty = discipline,
                        VocationEmployees = vacation,
                        SearchDate = time
                    };
                    return View(attandanceTable);
                }
            }
            catch(Exception exp)
            {
                ModelState.AddModelError("", exp.Message);
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
                    if(Calculate.CountIsBigNumberMin((int)count,discipline.MinDay))
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
                    await Calculate.EmployeePenaltySum(dbContext, count, empdata.Id,absent.DateTime);
                    ModelState.AddModelError("", "Success");
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
                    await Calculate.EmployeePenaltySum(dbContext, count, empdata.Id,absent.DateTime);
                    ModelState.AddModelError("", "Success");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> BonusEmployee(int? id)
        {
            if(id != null)
            {
                var datas = await dbContext.Employees
                                    .Where(x => x.Id == id)
                                          .Include(x=> x.AccuredSalaries)
                                               .FirstOrDefaultAsync();
                if(datas != null)
                {
                     if(!datas.AccuredSalaries.Any(x=> x.AccuredDate.Month == DateTime.Now.Month))
                    {
                        HttpContext.Session.SetObjectAsJson("Employ", datas);
                        return View();
                    }
                }
            }

            return RedirectToAction("MenecerEmployeeList", "Menecer",new { area = "Admin"});
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BonusEmployee(Bonus bonus)
        {
            if(ModelState.IsValid)
            {
                var menecer = HttpContext.Session.GetObjectFromJson<SessionUserModel>("UserData");
                var data =  HttpContext.Session.GetObjectFromJson<Employee>("Employ");
                if(data != null)
                {
                       if(!dbContext.EmployeAccuredSalaries.Any(x=> x.AccuredDate.Month == DateTime.Now.Month))
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
                                catch (Exception ex)
                                {
                                    ModelState.AddModelError("", ex.Message);
                                }
                            }

                        }
                       else
                        {
                            ModelState.AddModelError("", "You won't be able to write a bonus as this month's salary is calculated");
                        return View();
                    }
                }
            }
            return RedirectToAction("EmployeePositionTable","Menecer",new {area = "Admin"});
        }

        [HttpGet]
        public async Task<ActionResult> EmployeePositionTable()
        {
            List<MenecerEmployeeBonusModel> employeeBonusModels = new List<MenecerEmployeeBonusModel>();
            try
            {
                var menecer = HttpContext.Session.GetObjectFromJson<SessionUserModel>("UserData");
                if (menecer != null)
                {
                    var bonuses = await dbContext.Bonus
                            .Where(x => x.AppUserId == menecer.Id)
                                 .ToListAsync();

                    if (bonuses != null)
                    {
                        foreach (var emp in bonuses)
                        {
                            var employeeBonus = dbContext.Placeswork.Where(x => x.EmployeeId == emp.EmployeeId)
                                      .Include(x => x.Emporium)
                                         .Include(x => x.Positions)
                                            .Include(x => x.Employee)
                                               .ThenInclude(x => x.Bonus).Select(x => new MenecerEmployeeBonusModel
                                               {
                                                   Emporium = x.Emporium.Name,
                                                   Bonus = x.Employee.Bonus.Single().BonusPrize,
                                                   Position = x.Positions.Name,
                                                   Name = x.Employee.Name,
                                                   StartDate = x.StarDate,
                                                   EmployeeId = x.EmployeeId
                                               }).FirstOrDefault();

                            employeeBonusModels.Add(employeeBonus);
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

        [HttpGet]
        public async Task<ActionResult> AddVacation(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var data = await dbContext.Employees
                           .Where(x => x.Id == id)
                            .FirstOrDefaultAsync();

            if (data == null)
                 return NotFound();

            HttpContext.Session.SetObjectAsJson("Employ", data);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddVacation(VacationEmployee vacationEmployee)
        {
            if(ModelState.IsValid)
            {
               var data =  HttpContext.Session.GetObjectFromJson<Employee>("Employ");
                VacationEmployee vacation = new VacationEmployee
                {
                    CalcSalary = false,
                    StarDate = vacationEmployee.StarDate,
                    EndDate = vacationEmployee.EndDate,
                    EmployeeId = data.Id
                };
                await dbContext.VacationEmployees.AddAsync(vacation);
                await dbContext.SaveChangesAsync();
                ModelState.AddModelError("", "Success");
            }
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> BonusEdit(int? id)
        {
            if(id !=null)
            {
                
                var employe = dbContext.Employees.Where(x => x.Id == id).FirstOrDefault();
                HttpContext.Session.SetObjectAsJson("Employ",employe);
                var Bonusdata = await dbContext.Bonus.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();
                if (Bonusdata != null)
                {
                    return View(Bonusdata);
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BonusEdit(Bonus bonus)
        {
            if(ModelState.IsValid)
            {
                var data = HttpContext.Session.GetObjectFromJson<Employee>("Employ");
                var Bonusdata = await dbContext.Bonus
                                       .Where(x => x.EmployeeId == data.Id)
                                             .FirstOrDefaultAsync();
                if(Bonusdata != null)
                {
                    Bonusdata.BonusPrize = bonus.BonusPrize;
                    Bonusdata.BonusDate = bonus.BonusDate;
                    Bonusdata.BonusStatus = bonus.BonusStatus;
                    dbContext.Bonus.Update(Bonusdata);
                    await dbContext.SaveChangesAsync();
                    ModelState.AddModelError("", "Success");
                }
            }
            return View();
        }


        //Yazacaqsan Baxarsan buna yoxlamaq lazimdir    Departamente Role Verende
        [HttpGet]
        public async Task<ActionResult> BonusDel(int? id)
        {
            if (!id.HasValue)
                return NotFound();

           var data = await dbContext.Bonus
                          .Where(x => x.Id == id)
                             .FirstOrDefaultAsync();

            if (data == null)
                return BadRequest();

            dbContext.Bonus.Remove(data);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(EmployeePositionTable));
        }

        [HttpGet]
        public async Task<ActionResult> AdminMessageReciurment(int? id)
        {

            try
            {
                if (!id.HasValue)
                    return NotFound();

                var data = await dbContext.Employees
                            .Where(x => x.Id == id)
                                .FirstOrDefaultAsync();

                if (data == null)
                    return BadRequest();

                MessageReciurment messageReciurment = new MessageReciurment
                {
                    FirstName = data.Name,
                    SecondName = data.Surname,
                    Email = data.Email,
                    PhoneNumber = data.Number,
                    EmployeeId = (int)id
                };
                await dbContext.MessageReciurments.AddAsync(messageReciurment);
                await dbContext.SaveChangesAsync();
            }
            catch(Exception exp)
            {
                ModelState.AddModelError("", exp.Message);
            }
            return RedirectToAction("EmployeeAll","Reciurment",new { area = "Admin"});
        }

        [HttpGet]
        public  ActionResult AddMonthSale()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddMonthSale(EmporiumMonthSale emporiumMonth)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var dateMonth = DateTime.Now.Month;
                    var menecerId = HttpContext.Session.GetObjectFromJson<AppUser>("UserData");
                    var IsDateSale =  dbContext.EmporiumMonths
                                             .Where(x => x.Date.Month == dateMonth)
                                                    .FirstOrDefault();
                    if(IsDateSale == null)
                    {
                        var emporia = dbContext.EmporiumAppUsers
                                          .Where(x => x.AppUserId == menecerId.Id)
                                             .FirstOrDefault();

                        emporiumMonth.EmporiumId = emporia.EmporiumId;
                        await dbContext.EmporiumMonths.AddAsync(emporiumMonth);
                        await dbContext.SaveChangesAsync();
                        ModelState.AddModelError("", "Success");
                        return View();
                    }
                    ModelState.AddModelError("", "Monthly sales were recorded for this date");
                }
                catch(Exception exp)
                {
                    ModelState.AddModelError("", exp.Message);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> MonthSaleAjax()
        {
            List<MonthSaleModel> saleModel = new List<MonthSaleModel>();
            try
            {
                var menecerId = HttpContext.Session.GetObjectFromJson<AppUser>("UserData");

                saleModel = await dbContext.EmporiumAppUsers
                            .Where(x => x.AppUserId == menecerId.Id)
                                .Include(y => y.Emporium)
                                   .ThenInclude(x => x.MonthSales)
                                      .Select(x => new MonthSaleModel
                                      {
                                          Emporium = x.Emporium.Name,
                                          Prize = x.Emporium.MonthSales.First().Prize
                                      }).ToListAsync();
            }
            catch (Exception Exp)
            {
                ModelState.AddModelError("", Exp.Message);
            }
            return Json(new { message = 200 ,data = saleModel});
        }

    }
}