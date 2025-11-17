using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymTest.Persistence.Data;
using GymTest.Application.Services.Users.Command;
using GymTest.Application.Interfaces.Contexts;
using Microsoft.AspNetCore.Identity;
using GymTest.Domain.Entities.Users;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EndPoint.Site

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
            // جیمینای
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }) .AddCookie(options =>
{
                    options.LoginPath = "/Account/Registe"; // آدرس صفحه لاگین (حتی اگر هنوز نساخته‌اید)
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // مدت زمان اعتبار کوکی
             });

            //تا اینجا



            services.AddControllersWithViews();

            string contectionString = @"Data Source=DESKTOP-7SQIRAD; Initial Catalog=GymDB; Integrated Security=True;";
            //services.AddEntityFrameworkSqlServer().AddDbContext<DataBaseContext>(option => option.UseSqlServer(contectionString));

            //services.AddScoped<IDataBaseContext, DataBaseContext>();

            // ۱. ابتدا DbContext را ثبت می‌کنیم
            services.AddDbContext<DataBaseContext>(option => option.UseSqlServer(contectionString));

            // ۲. سپس به DI می‌گوییم که برای IDataBaseContext از همان نمونه ثبت شده استفاده کند
            services.AddScoped<IDataBaseContext>(provider => provider.GetService<DataBaseContext>());


            services.AddScoped<IRegisterUser, RegisterUser>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
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

            // جیمینای
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
