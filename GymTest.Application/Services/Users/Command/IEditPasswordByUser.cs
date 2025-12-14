using Common;
using GymTest.Application.Interfaces.Contexts;
using GymTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Users.Command
{
    public interface IEditPasswordByUser
    {
        ResultDto Execute(RequestChangePassword request);
    }

    public class EditPasswordByUser : IEditPasswordByUser
    {
        private readonly IDataBaseContext _context;

        public EditPasswordByUser(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestChangePassword request)
        {
            var target = _context.Users.Find(request.Id);

            if (target == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "کاربر مورد نظر یافت نشد"
                };
            }

            if (string.IsNullOrWhiteSpace(request.ExPassword))
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "لطفا پسورد فعلی خود را وارد کنید"
                };
            }

            var passwordHasher = new PasswordHasher();

            var verifyPassword = passwordHasher.VerifyPassword(target.Password, request.ExPassword);

            if (!verifyPassword)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "پسورد فعلی اشتباه است"
                };
            }


            if (!string.IsNullOrWhiteSpace(request.NewPassword) && request.NewPassword.Length < 5 )
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "پسورد جدید نمیتواند طولی کمتر از 5 کاراکتر داشته باشد"
                };
            }

            if (request.NewPassword != request.ReNewPassword)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "پسورد جدید و تکرار آن یکسان نیستند"
                };
            }

            if (request.NewPassword == request.ExPassword)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "پسورد جدید باید با پسورد فعلی متفاوت باشد"
                };

            }

            var hashedNewPassword = passwordHasher.HashPassword(request.NewPassword);

            target.Password = hashedNewPassword;
            target.UpdateTime = DateTime.Now;
            _context.SaveChanges();





            return new ResultDto()
            {
                IsSuccess = true,
                Message = "پسورد با موفقیت تغییر کرد"
            };

        }
    }

    public class RequestChangePassword
    {
        public long Id { get; set; }
        public string ExPassword { get; set; }
        public string NewPassword { get; set; }
        public string ReNewPassword { get; set; }
    }
}
