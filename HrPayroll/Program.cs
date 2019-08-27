using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HrPayroll.Core.SeedRun;
using HrPayroll.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HrPayroll
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
           IWebHost host = CreateWebHostBuilder(args).Build();
            using (IServiceScope scope = host.Services.CreateScope())
            {
                using (PayrollDbContext payrollDb = scope.ServiceProvider.GetRequiredService<PayrollDbContext>())
                {
                   await Seed.InvokeAsync(scope, payrollDb);
                }
            }
              await host.RunAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
