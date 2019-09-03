using HrPayroll.Areas.Admin.Models;
using HrPayroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Areas.Admin.AttendanceBool
{
    public static class BoolAttendance
    {
        public const string ItWorked = "ItWorked";
        public const string Sorry = "Sorry";
        public const string NotSorry = "NotSorry";


        public static async Task  BoolSaveSignInOut(PayrollDbContext payrollDb, SignInOutReasonTbl model,SignInOutReasonTbl signInOut )
        {
            if (BoolAttendance.Sorry == model.RaasonName)
            {
                signInOut.SignIn = false;
                await payrollDb.SignInOutReasons.AddAsync(signInOut);
                await payrollDb.SaveChangesAsync();
            }
            if (BoolAttendance.NotSorry == model.RaasonName)
            {
                signInOut.SignIn = false;
                await payrollDb.SignInOutReasons.AddAsync(signInOut);
                await payrollDb.SaveChangesAsync();
            }
            if (BoolAttendance.ItWorked == model.RaasonName)
            {
                signInOut.SignIn = true;
                await payrollDb.SignInOutReasons.AddAsync(signInOut);
                await payrollDb.SaveChangesAsync();
            }
        }
    }
}
