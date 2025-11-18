using Common;
using GymTest.Application.Interfaces.Contexts;
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
        ResultDto<ResultLoginUserDto>Execute();
    }

    public class LoginUser : ILoginUser
    {
        private readonly IDataBaseContext _context;
        public LoginUser(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ResultLoginUserDto> Execute()
        {
            
            // اگه خالی بودن بیاد ارور بده که پرش کن
            // اگه نبودن که بیا یه یوزر بساز و برو از دیتا بیس حایی مع اینیل ایمیل وارد شدس
            // و کاربر اکتیوه و... رو فرست اور دیفالت کن

            // اگه یوزر نال بود که بیا بیا ارور بده بگو ثبت نام نکردی

            //اگه اوکی بود بیا  چک ک ایا پسورد وارد شده درسته یا نه
            // اگه نبود دوباره ارور که رمزت ریده
            // اگه اوک بود
            // بیا یه ور رول تعریف کن که یه استرینگ خالیه بعد با یه فوریچ
            // بیا و تک تک ایتم هارو به ازای رول های یوزر اد کن توی وریبل رول
            // خروجی بده که اوکی شد رول و یوزر ای دی و نیمش هم به عنوان دیتا میدیم
            // کلاس ریزالت یوزر لاگینم که قبلا گذاشتم
            // در مورد کلیم ها هم توی کنترولر هندلش میکنیم

            
            throw new NotImplementedException();
        }

        public class ResultLoginUserDto
        {
            public string UserId { get; set; }
            public string Role { get; set; }
            public string Name { get; set; }

        }
    }
}
