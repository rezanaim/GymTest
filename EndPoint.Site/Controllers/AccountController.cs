using Common;
using GymTest.Application.Services.Users.Command;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static GymTest.Application.Services.Users.Command.LoginUser;

namespace EndPoint.Site.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRegisterUser _register;
        private readonly ILoginUser _login;

        public AccountController(IRegisterUser register, ILoginUser login)
        {

            _register = register;
            _login = login;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Register([FromBody]  RequestRegisterUserDto request)
        {


            
                // ۱. بررسی قوانین اعتبارسنجی ساده (Data Annotations)
                if (string.IsNullOrWhiteSpace(request.Email) ||
                    string.IsNullOrWhiteSpace(request.FirstName) ||
                    string.IsNullOrWhiteSpace(request.LastName) ||
                    string.IsNullOrWhiteSpace(request.Password) ||
                    string.IsNullOrWhiteSpace(request.RePassword))
                {
                    return Json(new ResultDto
                    {
                        IsSuccess = false,
                        Message = "لطفا همه موارد را پر کنید",
                    });
                }

                if (User.Identity.IsAuthenticated == true)
                {
                    return Json(new ResultDto()
                    {
                        IsSuccess = false,
                        Message = "شما وارد حساب کاربری خود شده اید، و امکان ثبت نام مجدد نیست"
                    });
                }

                if (request.Password != request.RePassword)
                {
                    return Json(new ResultDto()
                    {
                        IsSuccess = false,
                        Message = "پسورد و تکرار آن، یکسان نیستند"
                    });
                }

                if (request.Password.Length < 5)
                {
                    return Json(new ResultDto()
                    {
                        IsSuccess = false,
                        Message = "پسورد باید حداقل 5 کاراکتر باشد"
                    });
                }

                string emailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

                var match = Regex.Match(request.Email, emailRegex, RegexOptions.IgnoreCase);

                if (!match.Success)
                {
                    return Json(new ResultDto()
                    {
                        IsSuccess = false,
                        Message = "لطفا یک ایمیل معتبر وارد کنید"
                    });
                }
                // خ: اینا هم پیشنهاد جی پی تی بود و فعلا کامنت میشن
                // ۲. فراخوانی سرویس برای اجرای منطق اصلی کسب‌وکار
                var registerResult = await _register.Execute(new RequestRegisterUserDto()
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Password = request.Password,
                    RePassword = request.RePassword,
                    Roles = new List<RoleInRegisterUserDto>()
                    {
                        new RoleInRegisterUserDto{ /* Id = 3 */ },
                    }
                });

                if (registerResult.IsSuccess == false)
                {
                    return Json(registerResult);
                }

                if (registerResult.IsSuccess == true)
                    {
                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier,registerResult.Data.UserId.ToString()),
                            new Claim(ClaimTypes.Name, $"{request.FirstName} {request.LastName}" ),
                            new Claim(ClaimTypes.Email, request.Email),
                            //Id = 3 Ignored becuz of this line:
                            new Claim(ClaimTypes.Role, "Customer") 
                        };


                    // ازینجا تا اخر این ایف کلا کپی از فروشگاه
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties()
                    {
                        IsPersistent = true
                    };
                    await HttpContext.SignInAsync(principal, properties);

                }

            //return Json(registerResult);

            return Json(registerResult);
            // این پایینیا هم راهنمان

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



            //return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(RequestLoginUserDto request)
        {
            var resultLogin = await _login.Execute(new RequestLoginUserDto()
            {
                Email = request.Email,
                Password = request.Password
            });


            // ۲. بررسی نتیجه سرویس (مهم‌ترین بخش)
            if (resultLogin.IsSuccess == false)
            {
                // اگر لاگین ناموفق بود، نتیجه خطا را برگردان و از ادامه کار جلوگیری کن
                return Json(resultLogin);
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, resultLogin.Data.UserId.ToString()),
                //new Claim(ClaimTypes.Role, resultLogin.Data.Role.ToString()),
                new Claim(ClaimTypes.Name, $"{resultLogin.Data.FirstName} {resultLogin.Data.LastName}"),

                new Claim(ClaimTypes.Email, resultLogin.Data.Email),
                
                //foreach(var item in UserInRole.)

            };

            foreach (var item in resultLogin.Data.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }

            // ۱. ساخت هویت کاربر (کارت شناسایی) بر اساس کلیم‌ها و با استفاده از طرح احراز هویت کوکی.
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // ۲. ساخت هویت اصلی کاربر (کیف پول) که شامل کارت شناسایی اوست.
            var principal = new ClaimsPrincipal(identity);

            // ۳. تعریف ویژگی‌های کوکی، مانند ماندگاری (Remember Me) و تاریخ انقضا.
            var properties = new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.Now.AddDays(5),
            };

            
            // ۴. انجام عمل لاگین: رمزنگاری هویت کاربر در یک کوکی و ارسال آن به مرورگر.
            // اویت ر پاکیدم
            await HttpContext.SignInAsync(principal, properties);



            //temp return
            return Json(resultLogin);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home"); 

        }
    }
}
