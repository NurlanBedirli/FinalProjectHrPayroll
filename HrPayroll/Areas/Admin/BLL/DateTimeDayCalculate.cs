using HrPayroll.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.BLL
{
    public static class DateTimeDayCalculate
    {
        public static int GetDateDayMinus(List<VacationEmployee> vacations,DateTime nowMonth)
        {
            int sum = 0;
            foreach (var vacation in vacations)
            {
                if (vacation.StarDate.Month == nowMonth.Month || vacation.EndDate.Month == nowMonth.Month)
                {
                    sum += vacation.EndDate.Day - vacation.StarDate.Day;
                }
            }
            var total = sum.ToString();
            if(total.Contains('-'))
            {
              sum = Convert.ToInt32(total.Replace('-', ' '));
            }
            return sum;
        }


        public static bool IsDayAttandanceVacation(int nextday,int days,List<DateTime> signInOut, List<VacationEmployee> vacations)
        {
            bool isfoundDay = false;
            for (int i = nextday+1; i <= days; i++)
            {
                if (!signInOut.Any(x => x.Day == i))
                {
                    if (vacations.Any(x => x.StarDate.Day <= i) && vacations.Any(x => x.EndDate.Day >= i))
                    {
                        isfoundDay = false;
                    }
                    else
                    {
                        isfoundDay = true;
                        break;
                    }
                }
            }
            return isfoundDay;
        }

    }
}
