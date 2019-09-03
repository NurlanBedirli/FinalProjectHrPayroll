using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrPayroll.Areas.Admin.AdminCore;
using HrPayroll.Areas.Admin.EmployeeModel;
using HrPayroll.Areas.Admin.Models;
using HrPayroll.Areas.Admin.Options;
using HrPayroll.Areas.Admin.PaginationModel;
using HrPayroll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace HrPayroll.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReciurmentController : Controller
    {
        public PayrollDbContext dbContext;
        public readonly IOptions<WorkIdOptions> options;
       

        public ReciurmentController(PayrollDbContext _dbContext, IOptions<WorkIdOptions> _options)
        {
            dbContext = _dbContext;
            options = _options;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> EmployeeAll(int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            int elmPage = 5;
            var currentData = dbContext.Employees.ToList();
            var pageCount = Math.Ceiling(currentData.Count() / (decimal)elmPage);

            Paging paging1 = new Paging
            {
                CurrentPage = (int)page,
                ItemPage = elmPage,
                TotalItems = currentData.Count(),
                Prev = page > 1,
                Next = page < pageCount
            };


            var data = await dbContext.Employees.Skip((paging1.CurrentPage - 1) * paging1.ItemPage).Take(paging1.ItemPage).ToListAsync();

            PagingModel paging = new PagingModel
            {
                Employees = data,
                Paging = paging1
            };
            return View(paging);
        }

        [HttpGet]
        public async Task<ActionResult> AddNewWorkPlace(int? id)
        {
            if (id != null)
            {
                var data =  await dbContext.Placeswork.Where(x => x.EmployeeId == id).ToListAsync();
                if(data.Count ==0)
                {
                    var current = await dbContext.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();
                    if (current != null)
                    {
                        var holding = dbContext.Holdings.ToList();
                        HttpContext.Session.SetObjectAsJson("Holding", holding);
                        HttpContext.Session.SetObjectAsJson("Employ", current);
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("EmployeeAll", "Reciurment", new { area = "Admin" });
                }
               
            }
            return RedirectToAction("EmployeeAll","Reciurment",new { area = "Admin" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewWorkPlace(WorkPlaceModel workPlace) 
        {
            if (ModelState.IsValid)
            {
                var empId = HttpContext.Session.GetObjectFromJson<Employee>("Employ").Id;
                var empWorkPlace = await dbContext.Placeswork.Where(x => x.EmployeeId == empId).ToListAsync();
                if(empWorkPlace.Count == 0)
                {
                    var companyId = options.Value.CompanyId;
                    var positionsId = options.Value.PositionsId;
                    var emporiumId = options.Value.EmporiumId;
                    var salaryId = options.Value.SalaryId;

                    WorkPlace place = new WorkPlace
                    {
                        EmployeeId = empId,
                        EmporiumId = (int)emporiumId,
                        PositionsId = (int)positionsId,
                        StarDate = workPlace.StartDate
                    };
                    await dbContext.Placeswork.AddAsync(place);
                    await dbContext.SaveChangesAsync();
                    ModelState.AddModelError("", "Success");
                }
                else
                {
                    ModelState.AddModelError("", "You have already added a job ");
                }
               
            }
            return View();
        }

        public async Task<ActionResult> AllWorkPlaceEmployee()
        {
            List<EmployeeInfoWorkPlace> employeeInfo = new List<EmployeeInfoWorkPlace>();
            var infoEmployee = await dbContext.Placeswork.ToListAsync();
            foreach(var place in infoEmployee)
            {
                var emp = await dbContext.Employees.Where(x => x.Id == place.EmployeeId).FirstOrDefaultAsync();
                var emporia = await dbContext.Emporia.Where(x => x.Id == place.EmporiumId).FirstOrDefaultAsync();
                var company = await dbContext.Companies.Where(x => x.Id == emporia.CompanyId).FirstOrDefaultAsync();
                var positions = await dbContext.Positions.Where(x => x.Id == place.PositionsId).FirstOrDefaultAsync();
                var salary = await dbContext.EmployeeSalaries.Where(x => x.PositionsId == place.PositionsId).FirstOrDefaultAsync();
                DateTime date = place.StarDate;
                EmployeeInfoWorkPlace infoWorkPlace = new EmployeeInfoWorkPlace
                {
                    CompanyName = company.Name,
                    EmperiumName = emporia.Name,
                    PositionName = positions.Name,
                    PlasierCode = emp.PlasiyerCode,
                    Salary = salary.Salary,
                    Name = emp.Name + " " + emp.Surname,
                    Photo = emp.Photo,
                    StartDate = date,
                    id = emp.Id
                };
                employeeInfo.Add(infoWorkPlace);
            }
            var page = 1;
            int elmPage = 5;
            var pageCount = Math.Ceiling(employeeInfo.Count() / (decimal)elmPage);

            Paging paging1 = new Paging
            {
                CurrentPage = (int)page,
                ItemPage = elmPage,
                TotalItems = employeeInfo.Count(),
                Prev = page > 1,
                Next = page < pageCount
            };
            var pagingemployee = employeeInfo.Skip((page - 1) * elmPage).Take(elmPage).ToList();

            PagingModel paging = new PagingModel
            {
                EmployeeInfoWorks = pagingemployee,
                Paging = paging1
            };

            return View(paging);
        }

        //Ajax Dropdrown holding,company,emporium,position,salary
        public async Task<JsonResult> AjaxHoldingComBox(string value)
        {
            Holding holdings = await dbContext.Holdings.Include(c => c.Companies)
                    .Where(x => x.Name == value).Select(y => new Holding
                    {
                        Companies = y.Companies,
                        Id = y.Id,
                        Name = y.Name

                    }).FirstOrDefaultAsync();
          
            var companyName = holdings.Companies;
         

            return Json(new { company = companyName, message = 202 });
        }

        public async Task<JsonResult> AjaxEmporiumComboBox(int? id)
        {
           if (id !=null)
            {
                var data =  await dbContext.Emporia.Where(x => x.CompanyId == id).ToListAsync();
                if(data!=null)
                {
                    options.Value.CompanyId = id;   
                    return Json(new { emporia = data, message = 202 });
                }
                return Json(new { message = 404 });
            }
           else
            {
                return Json(new { message = 404 });
            }
            
        }

        public async Task<JsonResult> AjaxPositionsComboBox(int? id)
        {
            if (id != null)
            {
                List<Positions> positionsss = new List<Positions>();
                var data = await dbContext.EmporiumPositions.Where(x => x.EmporiumId == id).ToListAsync();
                if(data!=null)
                {
                    options.Value.EmporiumId = id;
                    foreach (var position in data)
                    {
                        
                        var positions = dbContext.Positions.Where(x => x.Id == position.PositionsId).Select(a => new Positions
                        {
                            Name = a.Name,
                            Id = a.Id
                        }).FirstOrDefault();
                        positionsss.Add(positions);
                    }

                    return Json(new { positionss = positionsss, message = 202 });
                }
                return Json(new { message = 404 });
            }
            else
            {
                return Json(new { message = 404 });
            }

        }

        public async Task<JsonResult> AjaxSalaryComboBox(int? id)
        {
            if(id != null)
            {
                //option Secret PositionId;
                options.Value.PositionsId = id;
                //option Secret SalaryId;
                var data = await dbContext.EmployeeSalaries.Where(x => x.PositionsId == id).Select(z=> new EmployeeSalary {
                  Salary = z.Salary,
                  Id = z.Id
              }).ToListAsync();
              foreach(var salari in data)
                {
                    options.Value.SalaryId = salari.Id;
                }
               var data2 = await dbContext.PositionsDepartaments.Where(x => x.PositionsId == id).FirstOrDefaultAsync();
               var departaments =  await dbContext.Departaments.Where(x => x.Id == data2.DepartamentId)
                       .Select(z=> new Departament {
                           Name = z.Name,
                           Id = z.Id

               }).ToListAsync();
                
                //option Secret DepartamentId;
                foreach (var depart in departaments)
                {
                    options.Value.DepartamentId = depart.Id;
                }
                return Json(new {departament = departaments, salary = data, message = 202 });
            }
            else
            {
                return Json(new { message = 404 });
            }
            
                

        }

        public async Task<JsonResult> PagingWorkPlace(int? id, int elmPage)
        {
            List<EmployeeInfoWorkPlace> employeeInfo = new List<EmployeeInfoWorkPlace>();
            var infoEmployee = await dbContext.Placeswork.ToListAsync();
            foreach (var place in infoEmployee)
            {
                var emp = await dbContext.Employees.Where(x => x.Id == place.EmployeeId).FirstOrDefaultAsync();
                var emporia = await dbContext.Emporia.Where(x => x.Id == place.EmporiumId).FirstOrDefaultAsync();
                var company = await dbContext.Companies.Where(x => x.Id == emporia.CompanyId).FirstOrDefaultAsync();
                var positions = await dbContext.Positions.Where(x => x.Id == place.PositionsId).FirstOrDefaultAsync();
                var salary = await dbContext.EmployeeSalaries.Where(x => x.PositionsId == place.PositionsId).FirstOrDefaultAsync();
                DateTime date = place.StarDate;

                EmployeeInfoWorkPlace infoWorkPlace = new EmployeeInfoWorkPlace
                {
                    CompanyName = company.Name,
                    EmperiumName = emporia.Name,
                    PositionName = positions.Name,
                    PlasierCode = emp.PlasiyerCode,
                    Salary = salary.Salary,
                    Name = emp.Name + " " + emp.Surname,
                    Photo = emp.Photo,
                    StartDate = date,
                    id = emp.Id
                };
                employeeInfo.Add(infoWorkPlace);
            }

            if (id == null)
            {
                id = 1;
            }
            var page = (int)id;
            if (elmPage == 0)
            {
                elmPage = 10;
            }
            int elmpage = elmPage;
            var currentData = dbContext.Employees.ToList();
            var pageCount = Math.Ceiling(employeeInfo.Count() / (decimal)elmpage);

            Paging paging1 = new Paging
            {
                CurrentPage = (int)page,
                ItemPage = elmpage,
                TotalItems = employeeInfo.Count(),
                Prev = page > 1,
                Next = page < pageCount
            };
             var pagingemployee = employeeInfo.Skip((page - 1) * elmpage).Take(elmpage).ToList();
             return Json(new {currentData = pagingemployee,message = 202,prevElement = paging1.Prev, nextElement = paging1.Next, pageCount = pageCount, currentPage = paging1.CurrentPage });
        }

        public async Task<JsonResult> SearchWorkPlace(string value,decimal salary,int elmPage )
        {
            int? page = 1;
          
            if(elmPage == 0)
            {
                elmPage = 10;
            }
            int elmpage = elmPage;
            decimal pageCount = 0;
            Paging paging = new Paging();
            paging.CurrentPage = (int)page;
            paging.ItemPage = elmpage;
            paging.Prev = page > 1;

            List<EmployeeInfoWorkPlace> places = new List<EmployeeInfoWorkPlace>();
            EmployeeWorkPlace employeeWork = new EmployeeWorkPlace();
            var currentdata = await employeeWork.EmployeeInfoWorks(dbContext);
           
          
            if (value != null)
            {

                var searchData = currentdata.Where(x => x.PositionName.Contains(value) || x.PositionName.StartsWith(value) ||
                           x.CompanyName.Contains(value) || x.CompanyName.StartsWith(value)
                                     || x.EmperiumName.Contains(value) || x.EmperiumName.StartsWith(value)
                                        || x.Name.Contains(value) || x.Name.StartsWith(value)
                                           || x.PlasierCode.Contains(value) || x.PlasierCode.StartsWith(value)
                                              || x.Salary == salary).ToList();

                paging.TotalItems = searchData.Count();
                pageCount = Math.Ceiling(searchData.Count() / (decimal)elmpage);
                paging.Next = page < pageCount;

                var datapaging = searchData.Skip((paging.CurrentPage - 1) * elmpage).Take(elmpage).ToList();
                return Json(new { pageCount = pageCount, currentPage = paging.CurrentPage, allData = searchData, nextElement = paging.Next, prevElement = paging.Prev, data=datapaging, message = 202 });
            }

            paging.TotalItems = currentdata.Count();
            pageCount = Math.Ceiling(currentdata.Count() / (decimal)elmpage);
            paging.Next = page < pageCount;
            var data = currentdata.Skip((paging.CurrentPage - 1) * elmpage).Take(elmpage).ToList();
            return Json(new { pageCount = pageCount, currentPage = paging.CurrentPage, allData = currentdata, nextElement = paging.Next, prevElement = paging.Prev, data = data, message = 202 });
        }

    }
}