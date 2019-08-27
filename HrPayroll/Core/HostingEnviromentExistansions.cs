using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Core
{
    public static class HostingEnviromentExistansions
    {
        public static string GetFolder(this IHostingEnvironment environment)
        {
            return Path.Combine(environment.WebRootPath, "img");
        }
    }
}
