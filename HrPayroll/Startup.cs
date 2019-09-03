using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrPayroll.Areas.Admin.Options;
using HrPayroll.Core;
using HrPayroll.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HrPayroll
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add servicesto the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFileNameGenerator, DateTimeFileGenerator>();

            services.AddAuthorization();

            services.AddDbContext<PayrollDbContext>(x =>
            {
                x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<AppUser, IdentityRole>()
                .AddDefaultTokenProviders()
                 .AddEntityFrameworkStores<PayrollDbContext>();

           
            

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //DropDrown Select Option configuration
            services.Configure<WorkIdOptions>(options => Configuration.Bind(options));

            services.AddAuthentication();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            

            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();
            services.AddSession();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

        
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "default",
                  template: "{area:exists}/{controller=Account}/{action=About}/{id?}"
                );
            });
            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
