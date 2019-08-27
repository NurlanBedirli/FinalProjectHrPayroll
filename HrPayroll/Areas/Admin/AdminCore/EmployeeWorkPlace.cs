using HrPayroll.Areas.Admin.EmployeeModel;
using HrPayroll.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.AdminCore
{
    public class EmployeeWorkPlace
    {
        public async Task<List<EmployeeInfoWorkPlace>> EmployeeInfoWorks(PayrollDbContext dbContext)
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
            return employeeInfo;
        }

    }
}
