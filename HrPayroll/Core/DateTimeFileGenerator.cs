using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Core
{
    public class DateTimeFileGenerator : IFileNameGenerator
    {
        public string GetFileName(string format)
        {
            return $"{Path.GetRandomFileName()}.{DateTime.Now.ToString("dd.mmmm.yy.HH.mm")}.{format}";
        }
    }
}
