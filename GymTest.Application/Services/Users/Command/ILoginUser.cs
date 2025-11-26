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
        Task<ResultDto<ResultLoginUserDto>>Execute (RequestLoginUserDto request);
    }

    public class LoginUser : ILoginUser

    {

        
        private readonly IDataBaseContext _context;
        public LoginUser(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task <ResultDto<ResultLoginUserDto>> Execute(RequestLoginUserDto request)
        {


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





            
            var user = await _context.Users
                .Include(p => p.UserInRoles)
                .ThenInclude(p => p.Role)
                .Where(p => p.Email.Equals(request.Email)
                && p.IsActive == true).FirstOrDefaultAsync();
            

            if (user == null)
            {
                return new ResultDto<ResultLoginUserDto>()
                {
                    IsSuccess = false,
                    Message = "کاربری با این مشخصات یافت نشد",
                    Data = new ResultLoginUserDto()
                    {

                    }
                };
            }


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

                    }
                };
            }

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


            // در مورد کلیم ها هم توی کنترولر هندلش میکنیم

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
