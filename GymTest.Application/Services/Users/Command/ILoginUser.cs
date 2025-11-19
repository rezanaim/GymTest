using Common;
using GymTest.Application.Interfaces.Contexts;
using GymTest.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GymTest.Application.Services.Users.Command.LoginUser;

namespace GymTest.Application.Services.Users.Command
{
    public interface ILoginUser
    {
        ResultDto<ResultLoginUserDto>Execute(RequestLoginUserDto request);
    }

    public class LoginUser : ILoginUser

    {

        
        private readonly IDataBaseContext _context;
        public LoginUser(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ResultLoginUserDto> Execute(RequestLoginUserDto request)
        {

            // اگه خالی بودن بیاد ارور بده که پرش کن
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return new ResultDto<ResultLoginUserDto>()
                {
                  
                    IsSuccess = false,
                    Message = "لطفا ایمیل خود را وارد کنید",
                    Data = new ResultLoginUserDto()
                    {
                        //UserId = 0,
                    }
                };
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return new ResultDto<ResultLoginUserDto>()
                {
                    IsSuccess = false,
                    Message = "لطفا پسورد خود را وارد کنید",
                    Data = new ResultLoginUserDto()
                    {
                        //UserId = 0,
                    }
                };
            }



            // اگه نبودن که بیا یه یوزر بساز و برو از دیتا بیس حایی مع اینیل ایمیل وارد شدس
            // و کاربر اکتیوه و... رو فرست اور دیفالت کن
            // اگه یوزر نال بود که بیا بیا ارور بده بگو ثبت نام نکردی

            
            var user = _context.Users
                .Include(p => p.UserInRoles)
                .ThenInclude(p => p.Role)
                .Where(p => p.Email.Equals(request.Email)
                && p.IsActive == true).FirstOrDefault();
            

            if (user == null)
            {
                return new ResultDto<ResultLoginUserDto>()
                {
                    IsSuccess = false,
                    Message = "کاربری با این مشخصات یافت نشد",
                    Data = new ResultLoginUserDto()
                    {
                        //UserId = 0,
                    }
                };
            }

            //اگه اوکی بود بیا  چک ک ایا پسورد وارد شده درسته یا نه
            // اگه نبود دوباره ارور که رمزت ریده



            /*
            var passwordHasher = new PasswordHasher();
            var hashedRequestPassword = passwordHasher.HashPassword(request.Password);
            
            if (hashedRequestPassword != user.Password)
            {
                return new ResultDto<ResultLoginUserDto>()
                {
                    IsSuccess = false,
                    Message = "پسورد وارد شده اشتباه است",
                    Data = new ResultLoginUserDto()
                    {
                        //UserId = 0,
                    }
                };
            }
            */

            // چک کردن پسورد به شکل بالا اشتباهه. دلیل؟
            // چون ما از رمزنگاری کاستوم و خاصی استفاده کردیم
            // برخلاف بعضی روش های ساده. اگر ما یک متن ایکس رو به عنوان پسورد در نظر بگیریم
            // هش شده ایکس.  برابر با  هش شده ایکس نیست
            // با این که ماده اولیه یکیه ولی هر بار هش کنی یه هش جدید و یونیک برات میسازیه ازش
            // راه هل؟ متد وریفای خودش

            var passwordHasher = new PasswordHasher();
            var vefifyPassword = passwordHasher.VerifyPassword(user.Password, request.Password);

            if (vefifyPassword == false)
            {
                return new ResultDto<ResultLoginUserDto>()
                {
                    IsSuccess = false,
                    Message = "پسورد وارد شده اشتباه است",
                    Data = new ResultLoginUserDto()
                    {
                        //UserId = 0,
                    }
                };
            }

            // اگه اوک بود
            // بیا یه ور رول تعریف کن که یه استرینگ خالیه بعد با یه فوریچ
            // بیا و تک تک ایتم هارو به ازای رول های یوزر اد کن توی وریبل رول


            var roles = new List<string>();
            foreach (var item in user.UserInRoles)
            {
                roles.Add(item.Role.Name);
            }


            return new ResultDto<ResultLoginUserDto>()
            {
                IsSuccess = true,
                Message = "لاگین با موفقیت انجام شد",
                Data = new ResultLoginUserDto()
                {
                    Roles = roles,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserId = user.Id,
                    Email = user.Email
                }
            };

            // خروجی بده که اوکی شد رول و یوزر ای دی و نیمش هم به عنوان دیتا میدیم
            // کلاس ریزالت یوزر لاگینم که قبلا گذاشتم
            // در مورد کلیم ها هم توی کنترولر هندلش میکنیم

            //temp return null
            //return null;
        }

        public class RequestLoginUserDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class ResultLoginUserDto
        {
            public long UserId { get; set; }
            public List<string> Roles { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }

        }
    }
}
