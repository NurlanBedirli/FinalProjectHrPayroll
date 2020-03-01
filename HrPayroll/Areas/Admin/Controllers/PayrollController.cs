using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrPayroll.Areas.Admin.EmployeeModel;
using HrPayroll.Models;
using HrPayroll.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HrPayroll.Areas.Admin.BLL;

namespace HrPayroll.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PayrollController : Controller
    {
        public  PayrollDbContext dbContext;

        public PayrollController(PayrollDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> EmployeeList()
        {
            var data = await EmployeeInfoSalary.EmployeeSalaryAsync(dbContext,DateTime.Now);
            return View(data);
        }

        [HttpPost]
        public async Task<ActionResult> EmployeeList(int []Id,DateTime DateTime,DateTime SerchDateTime)
        {
            List<EmployeeInfoWorkPlace> data = null;
            if(SerchDateTime.Year != 1 )
            {
                 data = await EmployeeInfoSalary.EmployeeSalaryAsync(dbContext, SerchDateTime);
            }
            else
            {
                data = await EmployeeInfoSalary.EmployeeSalaryAsync(dbContext, DateTime);
            }

            if (Id.Length != 0)
            {
                foreach(int id in Id)
                {
                   var employee = dbContext.Employees.Where(x => x.Id == id).FirstOrDefault();
                   var empAccoured =  dbContext.EmployeAccuredSalaries.Where(x => x.EmployeeId == id).ToList();

                    // iscinin maasi hesablanibmi artiq
                    if (!empAccoured.Any(x=> x.AccuredDate.Month == DateTime.Month))
                    {
                        if (await dbContext.Employees.AnyAsync(x => x.Id == id))
                        {
                            int month = DateTime.Month;
                            int days = DateTime.DaysInMonth(DateTime.Year, DateTime.Month);

                           //iscinin maasi
                          decimal salary = dbContext.Employees
                                    .Where(x => x.Id == id)
                                       .Include(x => x.WorkPlaces)
                                           .ThenInclude(y => y.Positions)
                                              .ThenInclude(a => a.EmployeeSalaries)
                                                 .AsNoTracking()
                                                    .First().WorkPlaces.Positions.EmployeeSalaries.Salary;



                            //magazanin ayliq kvotasi prize
                            var quataMenecer = dbContext.Placeswork
                                   .Where(x => x.EmployeeId == id)
                                      .Include(z => z.Emporium)
                                          .ThenInclude(v => v.EmporiumMonthSales)
                                             .Single().Emporium.EmporiumMonthSales.Where(b => b.Date.Month == month).FirstOrDefault();


                            // menecer kvota qeyd edibmi
                            if (quataMenecer == null)
                            {
                                ModelState.AddModelError("", "Admin has not set the monthly quota.");
                                return View(data);
                            }



                            //adminin ayliq qoydugu kvota
                            var quataAdmin = dbContext.Placeswork
                                    .Where(x => x.EmployeeId == id)
                                      .Include(a => a.Emporium)
                                         .ThenInclude(b => b.MonthSales)
                                             .Single()
                                                 .Emporium.MonthSales.Where(e => e.DateQuata.Month == month).FirstOrDefault();


                            // admin kvota qeyd edibmi
                            if (quataAdmin == null)
                            {
                                ModelState.AddModelError("", "The manager didn't record monthly sales");
                                return View(data);
                            }
                            // qvota bonus
                            decimal BonusQuata = 0;
                            if(quataAdmin.Quata < quataMenecer.Prize)
                            {
                                BonusQuata = quataAdmin.Prize;
                            }



                            //bonus
                            decimal bonus = dbContext.Employees
                                      .Where(x => x.Id == id)
                                         .Include(y => y.Bonus)
                                            .AsNoTracking()
                                              .First()
                                                 .Bonus
                                                    .Where(q => q.BonusDate.Month == month)
                                                       .Sum(x => x.BonusPrize);

                            //cerime admin terefinden
                            decimal penalty = dbContext.Employees
                                        .Where(x => x.Id == id)
                                           .Include(y => y.Penalties)
                                              .AsNoTracking()
                                                 .First()
                                                    .Penalties
                                                       .Where(q => q.Date.Month == month)
                                                         .Sum(c => c.Amount);



                            // ayliq mezuniyyetler 
                            List<VacationEmployee> vacations = dbContext.Employees
                                       .Where(x => x.Id == id)
                                          .Include(y => y.VacationEmployees)
                                             .AsNoTracking()
                                                 .First()
                                                    .VacationEmployees
                                                         .Where(x => x.StarDate.Month == month && x.EndDate.Month == month)
                                                            .ToList();

                            //ayliq mezuniyyetin sayi
                            var vacationDays = DateTimeDayCalculate.GetDateDayMinus(vacations, DateTime);


                            //Ayliq davamiyyet tarixi
                            var signInOut = dbContext.Employees.Where(x => x.Id == id)
                                   .Include(y => y.SignInOutReasonTbls)
                                        .First()
                                            .SignInOutReasonTbls
                                                  .Select(d => d.SignInTime)
                                                         .ToList();



                            //Ayliq cerime 
                            decimal attandancePenalty = dbContext.Employees
                                         .Where(x => x.Id == id)
                                            .Include(y => y.SignInOutReasonTbls)
                                               .AsNoTracking()
                                                  .First()
                                                     .SignInOutReasonTbls
                                                        .Where(q => q.SignInTime.Month == month)
                                                          .Sum(d => d.PenaltyAmount);

                            //Ayliq Davamiyyetin sayi
                            decimal attandance = dbContext.Employees
                                         .Where(x => x.Id == id)
                                            .Include(y => y.SignInOutReasonTbls)
                                               .AsNoTracking()
                                                  .First()
                                                     .SignInOutReasonTbls
                                                        .Where(q => q.SignInTime.Month == month)
                                                           .Where(z => z.SignIn == true)
                                                              .Count(v => v.SignIn);


                           //isci bu aydami ise qebul olub
                            DateTime date =  dbContext.Placeswork
                                 .Where(x => x.EmployeeId == id && x.StarDate.Month == month)
                                    .First().StarDate;
                             int nextday = 0;
                             if (date.Month == month)
                            {
                                nextday = date.Day;
                            }


                            if (!DateTimeDayCalculate.IsDayAttandanceVacation(nextday,days, signInOut, vacations))
                            {
                                decimal VacationSalary = Calculate.VacationSalary(salary, days, vacationDays);
                                decimal attandanceSalary = Calculate.AttandanceSalary(salary, days, attandance);

                                //cemi maas
                                decimal TotalSalary = Calculate.TotalSalary(BonusQuata, attandanceSalary, VacationSalary, bonus, penalty, attandancePenalty);

                                EmployeAccuredSalary accuredSalary = new EmployeAccuredSalary
                                {
                                    AccuredDate = DateTime.Now.Date,
                                    EmployeeId = id,
                                    Salary = TotalSalary
                                };
                                dbContext.EmployeAccuredSalaries.Add(accuredSalary);
                                await dbContext.SaveChangesAsync();
                            }
                            else
                            {
                                ModelState.AddModelError("", $"Manager {employee.Name}  {employee.Surname} has not made a note in the attendance schedule.");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", $"{employee.Name}  {employee.Surname} This employee's salary was calculated.");
                    }
                }
            }
            return View(data);
        }

        [HttpGet]
        public async Task<ActionResult> SalaryEmployeeList(DateTime DateTime)
        {
            await Task.Delay(0);
            return View();
        }

    }
}