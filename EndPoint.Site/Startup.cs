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
//using GymTest.Application.Services.Users.Query.IGetUser;
using GymTest.Application.Services.Users.Query.GetUser;
using GymTest.Application.Services.Users.Query.GetDataForUpdateUser;

namespace EndPoint.Site

{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });

            services.AddControllersWithViews();

            string connectionString = @"Data Source=DESKTOP-7SQIRAD; Initial Catalog=GymDB; Integrated Security=True;";

            // فقط این دو خط برای دیتابیس
            services.AddDbContext<DataBaseContext>(option => option.UseSqlServer(connectionString));
            services.AddScoped<IDataBaseContext>(provider => provider.GetService<DataBaseContext>());

            // ثبت سرویس‌ها
            services.AddScoped<IRegisterUser, RegisterUser>();
            services.AddScoped<ILoginUser, LoginUser>();
            services.AddScoped<IGetUsers, GetUser>();
            services.AddScoped<IDeleteUser, DeleteUser>();
            services.AddScoped<IUpdateUser, UpdateUser>();
            services.AddScoped<IGetDataForUpdateUser, GetDataForUpdateUser>();
            services.AddScoped<IAddNewCourse, AddNewCourse>();
            

        }
        // This method gets called by the runtime. Use this method to add services to the container.
        /*
         public void ConfigureServices(IServiceCollection services)
         {خ

             // جیمینای

             services.AddAuthentication(options =>
             {
                 options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                 options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                 options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
             }).AddCookie(options =>
            {
                options.LoginPath = "/Account/Login"; // آدرس صفحه لاگین (حتی اگر هنوز نساخته‌اید)
                 options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // مدت زمان اعتبار کوکی
             });

             //تا اینجا



             services.AddControllersWithViews();

             string contectionString = @"Data Source=DESKTOP-7SQIRAD; Initial Catalog=GymDB; Integrated Security=True;";
             services.AddEntityFrameworkSqlServer().AddDbContext<DataBaseContext>(option => option.UseSqlServer(contectionString));

             services.AddScoped<IDataBaseContext, DataBaseContext>();

             // ۱. ابتدا DbContext را ثبت می‌کنیم
             //services.AddDbContext<DataBaseContext>(option => option.UseSqlServer(contectionString));

             // ۲. سپس به DI می‌گوییم که برای IDataBaseContext از همان نمونه ثبت شده استفاده کند
             //services.AddScoped<IDataBaseContext>(provider => provider.GetService<DataBaseContext>());

             //services.AddScoped<IDataBaseContext, DataBaseContext>();
             services.AddScoped<IRegisterUser, RegisterUser>();
             services.AddScoped<ILoginUser, LoginUser>();
             services.AddScoped<IGetUsers, GetUser>();
             //services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
         }
         */
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
                    // ۱. این الگو باید اول باشد تا URLهای شامل Area را مدیریت کند
                    endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                    // ۲. این الگوی پیش‌فرض، برای URLهای بدون Area است و باید دوم باشد
                    endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}

//git test