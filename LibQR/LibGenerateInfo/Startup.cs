using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibGenerateInfo.Models;
using LibQRDAL.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibGenerateInfo
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
            services.AddMvc();
            //var connection = @"Server=.;Database=QR;Trusted_Connection=True;ConnectRetryCount=0";
            //services.AddDbContext<QRContext>(options => options.UseSqlServer(connection));
            var connection = @"Data Source=data.sqlite3;";
            services.AddDbContext<QRContext>(options => options.UseSqlite(connection));

            services
                    .AddAuthentication(
                        CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();   
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            //app.UseMiddleware<CustomAuth>();
            app.UseMvc(routes =>
            {
                
                routes.MapRoute(
                        name: "areas",
                        template: "{area:exists}/{controller=HomeAdmin}/{action=Index}/{id?}"
                      );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    
}
