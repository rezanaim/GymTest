using Common;
using GymTest.Application.Interfaces.Contexts;
using GymTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Users.Command
{
    public interface IUpdateUser
    {
        public ResultDto Execute(RequestUpdateDto request);
    }


    public class UpdateUser : IUpdateUser
    {
        private readonly IDataBaseContext _context;
        public UpdateUser(IDataBaseContext context)
        {
            _context = context;
        }


        public ResultDto Execute(RequestUpdateDto request)
        {
            var user = _context.Users.Find(request.Id);

            if (user == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "کاربر مورد نظر یافت نشد"
                };
            }

            if (string.IsNullOrWhiteSpace(request.FirstName))
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "لطفا نام را وارد کنید"
                };
            }

            if (string.IsNullOrWhiteSpace(request.LastName))
            {
                    return new ResultDto()
                    {
                        IsSuccess = false,
                        Message = " لطفا نام خانوادگی را وارد کنید"
                    };

            };

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "لطفا ایمیل را وارد کنید"
                };
            }

            
            //if (!string.IsNullOrWhiteSpace(request.Email))
            //{

                string emailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
                var match = Regex.Match(request.Email, emailRegex);

                if (!match.Success)
                {
                    return new ResultDto()
                    {
                        IsSuccess = false,
                        Message = "ایمیل وارده شده معتبر نیست"
                    };
                }
            //}
            var emailCheck = _context.Users.Any(p=> p.Email ==request.Email && p.Id !=request.Id);
            if (emailCheck)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "این ایمیل توسط کاربر دیگری استفاده شده است"
                };
            }

            if (!string.IsNullOrWhiteSpace(request.Password) && request.Password.Length < 5)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "پسورد باید حداقل 5 کاراکتر باشد"
                };
            }


            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var hasher = new PasswordHasher();

                var hashedPassword = hasher.HashPassword(request.Password);

                user.Password = hashedPassword;

            }


            
            user.UpdateTime = DateTime.Now;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.IsActive = request.IsActive;


            user.Email = request.Email;
            

            _context.SaveChanges();

            var result = new ResultDto()
            {
                IsSuccess = true,
                Message = $"کاربر مورد نظر با شناسه {user.Id} با موفقیت بروز شد"
            };

            return (result);
        }
    }

    public class RequestUpdateDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

    }
}
