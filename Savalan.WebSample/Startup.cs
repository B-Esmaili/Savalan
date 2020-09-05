using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Savalan.Web.Mvc;
using Savalan.Web.Mvc.Builder;
using System.Diagnostics;

namespace Savalan.WebSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddControllersWithViews().ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(
            //         new AssemblyPart(Assembly.LoadFile("/home/behnam/Documents/Documents/projects/Savalan/Savalan.Plugins/SamplePlugin/bin/Debug/netstandard2.0/SamplePlugin.dll"))
            // ));

            services.AddSavalanWithPlugins(new List<string>() { "/home/behnam/Documents/Documents/projects/Savalan/Savalan.Plugins/SamplePlugin/bin/Debug/netstandard2.0/SamplePlugin.dll" });
            services.AddAuthorization();
            services.AddCors((opt)=>{
                opt.AddPolicy("Default",b=>{
                    b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();
            app.UseCors("Default");

            app.UseAuthorization();

            app.UseSavalan();


            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});
        }
    }
}
