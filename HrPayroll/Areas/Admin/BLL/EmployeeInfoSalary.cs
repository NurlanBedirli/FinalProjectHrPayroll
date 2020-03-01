using HrPayroll.Areas.Admin.EmployeeModel;
using HrPayroll.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.BLL
{
    public class EmployeeInfoSalary
    {
        public static async Task<List<EmployeeInfoWorkPlace>> EmployeeSalaryAsync(PayrollDbContext dbContext, DateTime DateTime)
        {
            var data = await dbContext.Placeswork
                .Include(x => x.Emporium)
                    .ThenInclude(y => y.Company)
                         .ThenInclude(a => a.Holding)
                               .Include(b => b.Positions)
                                    .ThenInclude(c => c.EmployeeSalaries)
                                            .Include(d => d.Employee)
                                               .ThenInclude(z => z.AccuredSalaries)
                                                          .Select(w => new EmployeeInfoWorkPlace
                                                          {
                                                              Name = w.Employee.Name,
                                                              Photo = w.Employee.Photo,
                                                              CompanyName = w.Emporium.Company.Name,
                                                              EmperiumName = w.Emporium.Name,
                                                              PositionName = w.Positions.Name,
                                                              EmployeAccuredSalaries = w.Employee.AccuredSalaries.Where(x => x.AccuredDate.Month == DateTime.Month).ToList(),
                                                              Salary = w.Positions.EmployeeSalaries.Salary,
                                                              id = w.EmployeeId
                                                          }).ToListAsync();
            return data;
        }
    }
}
