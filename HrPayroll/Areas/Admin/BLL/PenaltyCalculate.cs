using HrPayroll.Areas.Admin.Models;
using HrPayroll.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.BLL
{
    public static class PenaltyCalculate
    {
        public static async Task  EmployeePenaltySum(this PayrollDbContext payrollDb,int? count,int? employeeId)
        {
            if(count != null)
            {
                var positions = await payrollDb.Placeswork.Where(x => x.EmployeeId == employeeId).FirstOrDefaultAsync();
                if(positions != null)
                {
                    var amount = await payrollDb.EmployeeSalaries.Where(x => x.PositionsId == positions.PositionsId).FirstOrDefaultAsync();
                    var WillBeFined = await payrollDb.DisciplinePenalties.FirstOrDefaultAsync();//olunacaq cerime
                    if(WillBeFined != null)
                    {
                        if(!CountIsBigNumber((int)count,WillBeFined.Name))
                        {
                          var employee =  await payrollDb.Employees.Where(x => x.Id == employeeId).FirstOrDefaultAsync();
                            payrollDb.Employees.Remove(employee);
                            await payrollDb.SaveChangesAsync();
                        }
                        else
                        {
                           var penaltyAmount =  SalaryPenalty(amount.Salary, (int)count, WillBeFined.PenaltyValue);
                            Penalty penalty = new Penalty
                            {
                                Amount = penaltyAmount,
                                Date = DateTime.Now.Date,
                                EmployeeId = (int)employeeId
                            };
                            await payrollDb.Penalties.AddAsync(penalty);
                            await payrollDb.SaveChangesAsync();
                        }
                    }
                }
            }
        }


        public static decimal SalaryPenalty(decimal amount,int count,int willBeFinedvalue)
        {
            var date = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            var DivideByMonth = amount / date;
            var penalty = (DivideByMonth * count) * willBeFinedvalue;
            return penalty;
        }





         public static bool CountIsBigNumber(int count,string value)
        {
            bool Iscount = false;
            List<int> vs = new List<int>();
            foreach (char c in value)
            {
                if (Char.IsNumber(c))
                {
                    var number = Convert.ToInt32(c.ToString());
                    vs.Add(number);
                }
            }
            foreach(var figure in vs)
            {
                if(figure < count)
                {
                    Iscount = true;
                    break;
                }
                if(figure > count)
                {
                    Iscount = false;
                    break;
                }
            }

            return Iscount;
        }
    }

}
