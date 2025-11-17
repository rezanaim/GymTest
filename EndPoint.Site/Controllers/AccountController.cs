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
                    //await HttpContext.SignInAsync(principal, properties);

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
    }
}
