using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HrPayroll.Areas.Admin.EmployeeModel;
using HrPayroll.Areas.Admin.Models;
using HrPayroll.Areas.Admin.PaginationModel;
using HrPayroll.Core;
using HrPayroll.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HrPayroll.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        public PayrollDbContext dbContext;
        private IHostingEnvironment hostingEnvironment;
        private IFileNameGenerator nameGenerator;

        public EmployeeController(PayrollDbContext _payrollDb
                                  , IHostingEnvironment _hostingEnvironment
                                   , IFileNameGenerator _nameGenerator)
        {
            dbContext = _payrollDb;
            hostingEnvironment = _hostingEnvironment;
            nameGenerator = _nameGenerator;
        }


        public IActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }




        //Employee edit delete info
        public async Task<ActionResult> AllEmployee(int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            var elmpage = 8;
            var datas = await dbContext.Employees.ToListAsync();
            var pagingCount = Math.Ceiling(datas.Count / (decimal)elmpage);
            Paging paging = new Paging
            {
                CurrentPage = (int)page,
                ItemPage = elmpage,
                TotalItems = datas.Count,
                Prev = page > 1,
                Next = page < pagingCount
            };

            var PagingData = datas.Skip((paging.CurrentPage - 1) * paging.ItemPage).Take(paging.ItemPage).ToList();

            PagingModel pagingModel = new PagingModel
            {
                Employees = PagingData,
                Paging = paging
            };
            return View(pagingModel);
        }
        [HttpGet]
        public async Task<ActionResult> EditEmployee(int? id)
        {
            if (id != null)
            {
                Employee data = await dbContext.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (data != null)
                {
                    return View(data);
                }
                else
                {
                    ModelState.AddModelError("", "User is not exists");
                }
            }
            else
            {
                ModelState.AddModelError("", "User is not exists");
            }
            return RedirectToAction(nameof(AllEmployee));
        }
        [HttpPost]
        public async Task<ActionResult> EditEmployee(Employee employee, IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                Employee data = dbContext.Employees.Where(x => x.Email == employee.Email).FirstOrDefault();
                if (data != null)
                {
                    if (Photo != null)
                    {
                        if (Photo.IsFilePhotoFormat())
                        {
                            string fullPath = hostingEnvironment.GetFolder();
                            string format = Photo.GetFileFormat();
                            string fileName = nameGenerator.GetFileName(format);
                            string fullFilePath = Path.Combine(fullPath, fileName);
                            ImageRemove.PhotoPathDelete(data.Photo, fullPath);
                            await Photo.SaveFileAsync(fullFilePath);

                            data.Name = employee.Name;
                            data.Surname = employee.Surname;
                            data.FatherName = employee.FatherName;
                            data.Gender = employee.Gender;
                            data.IDCardExparyDate = employee.IDCardExparyDate;
                            data.IDCardSerialNumber = employee.IDCardSerialNumber;
                            data.MaritalStatus = employee.MaritalStatus;
                            data.Nationally = employee.Nationally;
                            data.Number = employee.Number;
                            data.Photo = fileName;
                            data.BirthDay = employee.BirthDay;
                            data.DistrictRegistration = employee.DistrictRegistration;
                            data.Email = employee.Email;
                            data.PlasiyerCode = employee.PlasiyerCode;

                            dbContext.Update<Employee>(data);
                            await dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            ModelState.AddModelError("", "Photo is format (jpg,png)");
                        }
                    }
                    else
                    {
                        data.Name = employee.Name;
                        data.Surname = employee.Surname;
                        data.FatherName = employee.FatherName;
                        data.Gender = employee.Gender;
                        data.IDCardExparyDate = employee.IDCardExparyDate;
                        data.IDCardSerialNumber = employee.IDCardSerialNumber;
                        data.MaritalStatus = employee.MaritalStatus;
                        data.Nationally = employee.Nationally;
                        data.Number = employee.Number;
                        data.BirthDay = employee.BirthDay;
                        data.DistrictRegistration = employee.DistrictRegistration;
                        data.Email = employee.Email;
                        data.PlasiyerCode = employee.PlasiyerCode;

                        dbContext.Update<Employee>(data);
                        await dbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User is not exists");
                }
            }
            return View();
        }
        public async Task<ActionResult> DeleteEmployee(int? id)
        {
            if (id != null)
            {
                var current = await dbContext.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (current != null)
                {
                    if (current.Photo != null)
                    {
                        string fullPath = hostingEnvironment.GetFolder();
                        ImageRemove.PhotoPathDelete(current.Photo, fullPath);
                        dbContext.Remove(current);
                    }
                    else
                    {
                        dbContext.Remove(current);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(AllEmployee));
        }
        [HttpGet]
        public async Task<ActionResult> AboutEmployee(int? page)
        {
            if (page != null)
            {
                var employee = await dbContext.Employees.Where(x => x.Id == page).FirstOrDefaultAsync();
                if (employee != null)
                {
                    HttpContext.Session.SetObjectAsJson("Employ", employee);
                    var placework = await dbContext.Placeswork.Where(x => x.EmployeeId == page).FirstOrDefaultAsync();
                    var education = await dbContext.Educations.Where(x => x.EmployeeId == page).ToListAsync();
                    var oldworkplace = await dbContext.OldWorkPlaces.Where(x => x.EmployeeId == page).ToListAsync();
                    if (placework != null)
                    {
                        var emporia = await dbContext.Emporia.Where(x => x.Id == placework.EmporiumId).FirstOrDefaultAsync();
                        var company = await dbContext.Companies.Where(x => x.Id == emporia.CompanyId).FirstOrDefaultAsync();
                        var positions = await dbContext.Positions.Where(x => x.Id == placework.PositionsId).FirstOrDefaultAsync();
                        var departamentId = await dbContext.PositionsDepartaments.Where(x => x.PositionsId == positions.Id).FirstOrDefaultAsync();
                        var departament = await dbContext.Departaments.Where(x => x.Id == departamentId.DepartamentId).FirstOrDefaultAsync();
                        var salary = await dbContext.EmployeeSalaries.Where(x => x.PositionsId == placework.PositionsId).FirstOrDefaultAsync();
                        DateTime date = placework.StarDate;
                        EmployeeInfoWorkPlace employeeInfo = new EmployeeInfoWorkPlace
                        {
                            CompanyName = company.Name,
                            EmperiumName = emporia.Name,
                            PositionName = positions.Name,
                            Salary = salary.Salary,
                            DepartamentName = departament.Name,
                            StartDate = date,
                        };

                        AboutEmployeeInformationModel aboutEmployeeInformation = new AboutEmployeeInformationModel()
                        {
                            Educations = education,
                            Employees = employee,
                            OldWorkPlaces = oldworkplace,
                            InfoWorkPlaces = employeeInfo
                        };

                        return View(aboutEmployeeInformation);
                    }
                    AboutEmployeeInformationModel aboutEmployeeInformation1 = new AboutEmployeeInformationModel()
                    {
                        Educations = education,
                        Employees = employee,
                        OldWorkPlaces = oldworkplace,
                    };
                    return View(aboutEmployeeInformation1);
                }
               
                HttpContext.Session.SetObjectAsJson("Employ", employee);
                return View(employee);
            }
            return RedirectToAction(nameof(AllEmployee));
        }
        [HttpGet]
        public async Task<ActionResult> DeleteEducationEmployee(int? id)
        {
            if(id != null)
            {
               var edu = await dbContext.Educations.Where(x => x.Id == id).FirstOrDefaultAsync();
                if(edu !=null)
                {
                   dbContext.Remove(edu);
                   await  dbContext.SaveChangesAsync();
                    return RedirectToAction("AboutEmployee", "Employee", new { area = "Admin" });
                }
            }
            return RedirectToAction("AboutEmployee", "Reciurment", new { area = "Admin" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditEducationEmployee(Education education)
        {
            if(ModelState.IsValid)
            {
                int id = HttpContext.Session.GetObjectFromJson<Education>("Edu").Id;

               var edu = await dbContext.Educations.Where(x => x.Id == id).FirstOrDefaultAsync();
                if(edu !=null)
                {
                    edu.UniversityFaculty = education.UniversityFaculty;
                    edu.UniversityName = education.UniversityName;
                    edu.StartDate = education.StartDate;
                    edu.EndDate = education.EndDate;
                    edu.Status = education.Status;
                }
                dbContext.Update<Education>(edu);
                await dbContext.SaveChangesAsync();
               return RedirectToAction("AboutEmployee", "Employee", new { area = "Admin" });
            }
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> EditEducationEmployee(int? id)
        {
            if (id != null)
            {
                var edu = await dbContext.Educations.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (edu != null)
                {
                    HttpContext.Session.SetObjectAsJson("Edu", edu);
                    return View(edu);
                }
            }
            return RedirectToAction("AboutEmployee", "Reciurment", new { area = "Admin" });
        }
        [HttpGet]
        public async Task<ActionResult> EditOldWork(int? id)
        {
            if (id != null)
            {
               var oldwork = await dbContext.OldWorkPlaces.Where(x => x.Id == id).FirstOrDefaultAsync();
                if(oldwork != null)
                {
                    HttpContext.Session.SetObjectAsJson("oldwork", oldwork);
                    return View(oldwork);
                }
            }
                return View();
        }
        [HttpPost]
        public async Task<ActionResult> EditOldWork(OldWorkPlace workPlace)
        {
            if (ModelState.IsValid)
            {
                var oldworkId = HttpContext.Session.GetObjectFromJson<OldWorkPlace>("oldwork").Id;
                var oldwork = await dbContext.OldWorkPlaces.Where(x => x.Id == oldworkId).FirstOrDefaultAsync();
                if (oldwork != null)
                {
                    oldwork.WorkCompany = workPlace.WorkCompany;
                    oldwork.WorkPosition = workPlace.WorkPosition;
                    oldwork.WorkStatus = workPlace.WorkStatus;
                    oldwork.StarDate = workPlace.StarDate;
                    oldwork.EndDate = workPlace.EndDate;

                    dbContext.Update<OldWorkPlace>(oldwork);
                    await  dbContext.SaveChangesAsync();
                    return View(oldwork);
                }
                return RedirectToAction("AboutEmployee", "Employee", new { area = "Admin" });
            }
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> DeleteOldWork(int? id)
        {
            if (id != null)
            {
                var oldwork = await dbContext.OldWorkPlaces.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (oldwork != null)
                {
                    dbContext.Remove(oldwork);
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction("AboutEmployee", "Employee", new { area = "Admin" });
                }
            }
            return RedirectToAction("AboutEmployee", "Reciurment", new { area = "Admin" });
        }

        // Employee  add  info
        [HttpGet]
        public ActionResult AddEmployee()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEmployee(Employee employee, IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                var data = dbContext.Employees.Where(x => x.Email == employee.Email).FirstOrDefault();

                if (data == null)
                {
                    if (Photo != null)
                    {
                        if (Photo.IsFilePhotoFormat())
                        {
                            string fullPath = hostingEnvironment.GetFolder();
                            string format = Photo.GetFileFormat();
                            string fileName = nameGenerator.GetFileName(format);
                            string fullFilePath = Path.Combine(fullPath, fileName);
                            await Photo.SaveFileAsync(fullFilePath);

                            dbContext.Employees.Add(new Employee
                            {
                                Name = employee.Name,
                                Surname = employee.Surname,
                                FatherName = employee.FatherName,
                                BirthDay = employee.BirthDay,
                                IDCardExparyDate = employee.IDCardExparyDate,
                                IDCardSerialNumber = employee.IDCardSerialNumber,
                                Email = employee.Email,
                                Gender = employee.Gender,
                                MaritalStatus = employee.MaritalStatus,
                                DistrictRegistration = employee.DistrictRegistration,
                                Nationally = employee.Nationally,
                                Number = employee.Number,
                                Photo = fileName,
                                PlasiyerCode = employee.PlasiyerCode
                            });
                            await dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            ModelState.AddModelError("", "Photo is format (jpg,png)");
                        }
                    }
                    else
                    {
                        dbContext.Employees.Add(new Employee
                        {
                            Name = employee.Name,
                            Surname = employee.Surname,
                            FatherName = employee.FatherName,
                            BirthDay = employee.BirthDay,
                            IDCardExparyDate = employee.IDCardExparyDate,
                            IDCardSerialNumber = employee.IDCardSerialNumber,
                            Email = employee.Email,
                            Gender = employee.Gender,
                            MaritalStatus = employee.MaritalStatus,
                            DistrictRegistration = employee.DistrictRegistration,
                            Nationally = employee.Nationally,
                            Number = employee.Number,
                            PlasiyerCode = employee.PlasiyerCode,
                        });
                        await dbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "This is email exists");
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> AddEmployeeOldWorkPlace(int? id)
        {
            if (id != null)
            {
                var data = await dbContext.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (data != null)
                {
                    HttpContext.Session.SetObjectAsJson("Employ", data);
                    return View();
                }
                else
                {
                    return RedirectToAction(nameof(EmployeeList));
                }
            }
            else
            {
                return RedirectToAction(nameof(EmployeeList));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEmployeeOldWorkPlace(OldWorkPlace oldWork)
        {
            if (ModelState.IsValid)
            {
                var data = HttpContext.Session.GetObjectFromJson<Employee>("Employ");

                dbContext.OldWorkPlaces.Add(new OldWorkPlace
                {
                    WorkCompany = oldWork.WorkCompany,
                    WorkPosition = oldWork.WorkPosition,
                    StarDate = oldWork.StarDate,
                    EndDate = oldWork.EndDate,
                    WorkStatus = oldWork.WorkStatus,
                    EmployeeId = data.Id
                });

                await dbContext.SaveChangesAsync();
                ModelState.AddModelError("", "Success");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEmployeeEducation(Education education)
        {
            if (ModelState.IsValid)
            {
                var data = HttpContext.Session.GetObjectFromJson<Employee>("Employ");

                dbContext.Educations.Add(new Education
                {
                    UniversityName = education.UniversityName,
                    UniversityFaculty = education.UniversityFaculty,
                    StartDate = education.StartDate.Date,
                    EndDate = education.EndDate.Date,
                    Status = education.Status,
                    EmployeeId = data.Id
                });
                await dbContext.SaveChangesAsync();
                ModelState.AddModelError("", "Success");
            }
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> AddEmployeeEducation(int? id)
        {
            if (id != null)
            {
                var data = await dbContext.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (data != null)
                {
                    HttpContext.Session.SetObjectAsJson("Employ", data);
                    return View();
                }
                else
                {
                    return RedirectToAction(nameof(EmployeeList));
                }
            }
            else
            {
                return RedirectToAction(nameof(EmployeeList));
            }
        }
        public async Task<ActionResult> EmployeeList(int? page)
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

        // Employee  searc filter pagination ajax
        [HttpPost]
        public async Task<JsonResult> AllEmployeeSearchAjax(Employee emp,int elmPage)
        {
            var  page = 1;
            if(elmPage == 0)
            {
                elmPage = 8;
            }
            int elmpage = elmPage;
            decimal pageCount = 0;
            Paging paging1 = new Paging();
            paging1.CurrentPage = (int)page;
            paging1.ItemPage = elmpage;

            var AllData = dbContext.Employees.ToList();
            pageCount = Math.Ceiling(AllData.Count() / (decimal)elmpage);
            paging1.Next = page < pageCount;

            if (emp.Name != null || emp.Surname !=null)
            {
                var currentData = await dbContext.Employees
                                           .Where(x => (x.Name.Contains(emp.Name)) | (x.Surname.Contains(emp.Surname))).ToListAsync();
                pageCount = Math.Ceiling(currentData.Count() / (decimal)elmpage);
                paging1.TotalItems = currentData.Count();
                paging1.Prev = page > 1;
                paging1.Next = page < pageCount;

                var data = currentData.Skip((paging1.CurrentPage - 1) * paging1.ItemPage).Take(paging1.ItemPage).ToList();
           return Json(new { pageCount = pageCount, currentPage = paging1.CurrentPage, allData = currentData, nextElement = paging1.Next, prevElement = paging1.Prev, currentData = data, message = 202 });
            }


            return Json(new { pageCount = pageCount, currentPage = paging1.CurrentPage, allData = AllData, nextElement = paging1.Next, prevElement = paging1.Prev, currentData = AllData.
                                                                        Skip((paging1.CurrentPage - 1) * paging1.ItemPage)
                                                                                       .Take(paging1.ItemPage)
                                                                                                      .ToList(), message = 202 });
        }
        [HttpPost]
        public async Task<JsonResult> AllTableEmployee(string name, string surname, string plasier,int elmPage)
       {
            int? page = 1;
            if(elmPage == 0)
            {
                elmPage = 10;
            }
            int elmpage = elmPage;
            Paging paging = new Paging();
            paging.CurrentPage = (int)page;
            paging.ItemPage = elmpage;
            paging.Prev = page > 1;

            var listData = await dbContext.Employees.ToListAsync();
            paging.TotalItems = listData.Count();
            var pageCount = Math.Ceiling(listData.Count() / (decimal)elmpage);
            paging.Next = page < pageCount;

            if (name != null)
            {
                var dataa = await dbContext.Employees.Where(x => x.Name.Contains(name) || x.Name.StartsWith(name)).ToListAsync();
                paging.TotalItems = dataa.Count();
                pageCount = Math.Ceiling(dataa.Count() / (decimal)elmpage);
                paging.Next = page < pageCount;
                if (!string.IsNullOrEmpty(surname))
                {
                    var data1 = dataa.Where(y => y.Surname.Contains(surname) || y.Surname.StartsWith(surname)).ToList();
                    paging.TotalItems = data1.Count();
                    pageCount = Math.Ceiling(data1.Count() / (decimal)elmpage);
                    paging.Next = page < pageCount;
                    if (!string.IsNullOrEmpty(plasier))
                    {
                        var data2 = data1.Where(y => y.PlasiyerCode.Contains(plasier) || y.PlasiyerCode.StartsWith(plasier)).ToList();
                        paging.TotalItems = data2.Count();
                        pageCount = Math.Ceiling(data2.Count() / (decimal)elmpage);
                        paging.Next = page < pageCount;
                        return Json(new { currentPage = paging.CurrentPage, nextElement = paging.Next, prevElement = paging.Prev, allData = data2,  zz = paging.PageCount, data = data2.Skip((paging.CurrentPage - 1) * paging.ItemPage).Take(paging.ItemPage).ToList(), message = 202 });
                    }
                    return Json(new { currentPage = paging.CurrentPage, nextElement = paging.Next, prevElement = paging.Prev, allData = data1, zz = paging.PageCount, data = data1.Skip((paging.CurrentPage - 1) * paging.ItemPage).Take(paging.ItemPage).ToList(), message = 202 });
                }
                if(!string.IsNullOrEmpty(plasier))
                {
                    var dataSurPlas = dataa.Where(y => y.PlasiyerCode.Contains(plasier) || y.PlasiyerCode.StartsWith(plasier)).ToList();
                    pageCount = Math.Ceiling(dataSurPlas.Count() / (decimal)elmpage);
                    paging.Next = page < pageCount;

                    paging.TotalItems = dataSurPlas.Count();
                    if (!string.IsNullOrEmpty(surname))
                    {
                        var dataplasier = dataSurPlas.Where(y => y.Surname.Contains(surname) || y.Surname.StartsWith(surname)).ToList();
                        pageCount = Math.Ceiling(dataplasier.Count() / (decimal)elmpage);
                        paging.Next = page < pageCount;
                        paging.TotalItems = dataplasier.Count();
                        return Json(new { currentPage = paging.CurrentPage, nextElement = paging.Next, prevElement = paging.Prev, allData = dataplasier, zz = paging.PageCount, data = dataplasier.Skip((paging.CurrentPage - 1) * paging.ItemPage).Take(paging.ItemPage).ToList(), message = 202 });
                    }
                    return Json(new { currentPage = paging.CurrentPage, nextElement = paging.Next, prevElement = paging.Prev, allData = dataSurPlas, zz = paging.PageCount, data = dataSurPlas.Skip((paging.CurrentPage - 1) * paging.ItemPage).Take(paging.ItemPage).ToList(), message = 202 });
                }
                return Json(new { currentPage = paging.CurrentPage, nextElement = paging.Next, prevElement = paging.Prev, allData = dataa, zz = paging.PageCount, data = dataa.Skip((paging.CurrentPage -1)*paging.ItemPage).Take(paging.ItemPage).ToList(), message = 202 });
            }
            if (surname != null)
            {
                var dataa = await dbContext.Employees.Where(x => x.Surname.Contains(surname) || x.Surname.StartsWith(surname)).ToListAsync();
                pageCount = Math.Ceiling(dataa.Count() / (decimal)elmpage);
                paging.Next = page < pageCount;
                paging.TotalItems = dataa.Count();
               
                if (!string.IsNullOrEmpty(name))
                {
                    var data1 = dataa.Where(y => y.Name.Contains(name) || y.Name.StartsWith(name)).ToList();
                    pageCount = Math.Ceiling(data1.Count() / (decimal)elmpage);
                    paging.Next = page < pageCount;
                    paging.TotalItems = data1.Count();
                    
                    if (!string.IsNullOrEmpty(plasier))
                    {
                        var data2 = data1.Where(y => y.PlasiyerCode.Contains(plasier) || y.PlasiyerCode.StartsWith(plasier)).ToList();
                        pageCount = Math.Ceiling(data2.Count() / (decimal)elmpage);
                        paging.Next = page < pageCount;
                        paging.TotalItems = data2.Count();
                        return Json(new { currentPage = paging.CurrentPage, nextElement = paging.Next, prevElement = paging.Prev, allData = data2, zz = paging.PageCount, data = data2.Skip((paging.CurrentPage - 1) * paging.ItemPage).Take(paging.ItemPage).ToList(), message = 202 });
                    }
                    return Json(new { currentPage = paging.CurrentPage, nextElement = paging.Next, prevElement = paging.Prev, allData = data1, zz = paging.PageCount, data = data1.Skip((paging.CurrentPage - 1) * paging.ItemPage).Take(paging.ItemPage).ToList(), message = 202 });
                }
                if (!string.IsNullOrEmpty(plasier))
                {
                    var dataSurPlas = dataa.Where(y => y.PlasiyerCode.Contains(plasier) || y.PlasiyerCode.StartsWith(plasier)).ToList();
                    pageCount = Math.Ceiling(dataSurPlas.Count() / (decimal)elmpage);
                    paging.Next = page < pageCount;
                    paging.TotalItems = dataSurPlas.Count();
                    if (!string.IsNullOrEmpty(name))
                    {
                        var dataplasier = dataSurPlas.Where(y => y.Name.Contains(name) || y.Name.StartsWith(name)).ToList();
                        pageCount = Math.Ceiling(dataplasier.Count() / (decimal)elmpage);
                        paging.Next = page < pageCount;
                        paging.TotalItems = dataplasier.Count();
                        return Json(new { currentPage = paging.CurrentPage, nextElement = paging.Next, prevElement = paging.Prev, allData = dataplasier, zz = paging.PageCount, data = dataplasier.Skip((paging.CurrentPage - 1) * paging.ItemPage).Take(paging.ItemPage).ToList(), message = 202 });
                    }
                    return Json(new { currentPage = paging.CurrentPage, nextElement = paging.Next, prevElement = paging.Prev, allData = dataSurPlas, zz = paging.PageCount, data = dataSurPlas.Skip((paging.CurrentPage - 1) * paging.ItemPage).Take(paging.ItemPage).ToList(), message = 202 });
                }
                return Json(new { currentPage = paging.CurrentPage, nextElement = paging.Next, prevElement = paging.Prev, allData = dataa, zz = paging.PageCount, data = dataa.Skip((paging.CurrentPage - 1) * paging.ItemPage).Take(paging.ItemPage).ToList(), message = 202 });
            }
            if (plasier != null)
            {
                var dataa = await dbContext.Employees.Where(x => x.PlasiyerCode.Contains(plasier) || x.PlasiyerCode.StartsWith(plasier)).ToListAsync();
                pageCount = Math.Ceiling(dataa.Count() / (decimal)elmpage);
                paging.Next = page < pageCount;
                paging.TotalItems = dataa.Count();
                if (!string.IsNullOrEmpty(name))
                {
                    var data1 = dataa.Where(y => y.Name.Contains(name) || y.Name.StartsWith(name)).ToList();
                    pageCount = Math.Ceiling(data1.Count() / (decimal)elmpage);
                    paging.Next = page < pageCount;
                    paging.TotalItems = data1.Count();
                    if (!string.IsNullOrEmpty(surname))
                    {
                        var data2 = data1.Where(y => y.Surname.Contains(surname) || y.Surname.StartsWith(surname)).ToList();
                        pageCount = Math.Ceiling(data2.Count() / (decimal)elmpage);
                        paging.Next = page < pageCount;
                        paging.TotalItems = data2.Count();
                        return Json(new { currentPage = paging.CurrentPage, nextElement = paging.Next, prevElement = paging.Prev, allData = data2, zz = paging.PageCount, data = data2.Skip((paging.CurrentPage - 1) * paging.ItemPage).Take(paging.ItemPage).ToList(), message = 202 });
                    }
                    return Json(new { currentPage = paging.CurrentPage, nextElement = paging.Next, prevElement = paging.Prev, allData = data1, zz = paging.PageCount, data = data1.Skip((paging.CurrentPage - 1) * paging.ItemPage).Take(paging.ItemPage).ToList(), message = 202 });
                }
                if (!string.IsNullOrEmpty(surname))
                {
                    var dataSurPlas = dataa.Where(y => y.Surname.Contains(surname) || y.PlasiyerCode.StartsWith(surname)).ToList();
                    pageCount = Math.Ceiling(dataSurPlas.Count() / (decimal)elmpage);
                    paging.Next = page < pageCount;
                    paging.TotalItems = dataSurPlas.Count();
                    if (!string.IsNullOrEmpty(name))
                    {
                        var dataplasier = dataSurPlas.Where(y => y.Name.Contains(name) || y.Name.StartsWith(name)).ToList();
                        pageCount = Math.Ceiling(dataplasier.Count() / (decimal)elmpage);
                        paging.Next = page < pageCount;
                        paging.TotalItems = dataplasier.Count();
                        return Json(new { currentPage = paging.CurrentPage, nextElement = paging.Next, prevElement = paging.Prev, allData = dataplasier, zz = paging.PageCount, data = dataplasier.Skip((paging.CurrentPage - 1) * paging.ItemPage).Take(paging.ItemPage).ToList(), message = 202 });
                    }
                    return Json(new { currentPage = paging.CurrentPage, nextElement = paging.Next, prevElement = paging.Prev, allData = dataSurPlas, zz = paging.PageCount, data = dataSurPlas.Skip((paging.CurrentPage - 1) * paging.ItemPage).Take(paging.ItemPage).ToList(), message = 202 });
                }
                return Json(new { currentPage = paging.CurrentPage, nextElement = paging.Next, prevElement = paging.Prev, allData = dataa, zz = paging.PageCount, data = dataa.Skip((paging.CurrentPage - 1) * paging.ItemPage).Take(paging.ItemPage).ToList(), message = 202 });
            }
             
            return Json(new { currentPage = paging.CurrentPage, nextElement = paging.Next, prevElement = paging.Prev, allData = listData, zz = paging.PageCount, data = listData.Skip((paging.CurrentPage - 1) * paging.ItemPage).Take(paging.ItemPage).ToList(), message = 202 });
        }
        [HttpPost]
        public async Task<JsonResult> PagingAjax(string count, int elmPage)
        {
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
            var currentData = dbContext.Employees.ToList();
            var pageCount = Math.Ceiling(currentData.Count() / (decimal)elmpage);/* 150/15=10*/
            Paging paging1 = new Paging
            {
                CurrentPage = (int)page,
                ItemPage = elmpage,
                TotalItems = currentData.Count(),
                Prev = page > 1,
                Next = page < pageCount
            };

            var data = await dbContext.Employees.Skip((paging1.CurrentPage - 1) * paging1.ItemPage).Take(paging1.ItemPage).ToListAsync();

            return Json(new { pageCount = pageCount, currentPage = paging1.CurrentPage, nextElement = paging1.Next, prevElement = paging1.Prev, currentData = data, message = 202 });
        }
    }
}