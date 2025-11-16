using GymTest.Application.Services.Users.Command;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EndPoint.Site.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRegisterUser _register;

        public AccountController(IRegisterUser register)
        {
            _register = register;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RequestRegisterUserDto request)
        {


            {
                // ۱. بررسی قوانین اعتبارسنجی ساده (Data Annotations)
                if (!ModelState.IsValid)
                {
                    // اگر ورودی کاربر معتبر نیست (مثلاً پسورد کوتاه است)،
                    // همان فرم را با پیام‌های خطا به او برگردان.
                    return View(request);
                }

                // ۲. فراخوانی سرویس برای اجرای منطق اصلی کسب‌وکار
                var result = _register.Execute(new RequestRegisterUserDto()
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Password = request.Password,
                    RePassword = request.RePassword,
                    Roles = new List<RoleInRegisterUserDto>()
                    {
                        new RoleInRegisterUserDto{ Id = 3 },
                    }
                });


                /*if (signeupResult.IsSuccess == true)
                {
                    var claims = new List<Claim>()
                {
                new Claim(ClaimTypes.NameIdentifier,signeupResult.Data.UserId.ToString()),
                new Claim(ClaimTypes.Email, request.Email),
                new Claim(ClaimTypes.Name, request.FullName),
                new Claim(ClaimTypes.Role, "Customer"),
                };


                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties()
                    {
                        IsPersistent = true
                    };
                    HttpContext.SignInAsync(principal, properties); */

                    // ۳. تصمیم‌گیری بر اساس نتیجه‌ای که از سرویس برگشته
                    if (result.IsSuccess)
                    {
                    // اگر ثبت‌نام موفقیت‌آمیز بود، کاربر را به صفحه اصلی سایت هدایت کن.
                    return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                    // اگر سرویس با یک خطای کسب‌وکار مواجه شد (مثلاً ایمیل تکراری)،
                    // آن پیام خطا را به کاربر نمایش بده و دوباره فرم را نشان بده.
                    foreach (var item in result.Message.Split(','))
                    {
                        ModelState.AddModelError(string.Empty, item);
                    }
                        return View(request);
                    }
            }

            //return RedirectToAction("Index", "Home");
        }
    }
}
