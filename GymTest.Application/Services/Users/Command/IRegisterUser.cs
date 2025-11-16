using Common;
using GymTest.Application.Interfaces.Contexts;
using GymTest.Common;
using GymTest.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static GymTest.Application.Services.Users.Command.UserRegister;

namespace GymTest.Application.Services.Users.Command
{
    interface IRegisterUser
    {
        ResultDto<ResultRegisterUserto> Execute(RequestRegisterUserDto request);
    }
    public class UserRegister : IRegisterUser
    {

        private readonly IDataBaseContext _context;
        public UserRegister(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultRegisterUserto> Execute(RequestRegisterUserDto request)
        {
            try
            {



                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return new ResultDto<ResultRegisterUserto>()
                    {
                        Data = new ResultRegisterUserto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "لطفا ایمیل خود را وارد کنید"
                    };
                }

                if (string.IsNullOrWhiteSpace(request.FirstName))
                {
                    return new ResultDto<ResultRegisterUserto>()
                    {
                        Data = new ResultRegisterUserto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "لطفا نام خود را وارد کنید"
                    };
                }

                if (string.IsNullOrWhiteSpace(request.LastName))
                {
                    return new ResultDto<ResultRegisterUserto>()
                    {
                        Data = new ResultRegisterUserto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "لطفا نام خانوادگی خود را وارد کنید"
                    };
                }

                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    new ResultDto<ResultRegisterUserto>()
                    {
                        Data = new ResultRegisterUserto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "لطفا یک پسورد انتخاب کنید"
                    };
                }

                if (request.Password != request.RePassword)
                {
                    new ResultDto<ResultRegisterUserto>()
                    {
                        Data = new ResultRegisterUserto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "پسورد و تکرار آن، یکسان نیستند"

                    };
                }
                // چک فرمت ایمیل
                string emailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
                var match = Regex.Match(request.Email, emailRegex, RegexOptions.IgnoreCase);

                if (!match.Success)
                {
                    return new ResultDto<ResultRegisterUserto>()
                    {
                        Data = new ResultRegisterUserto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "لطفا یک ایمیل معتبر وارد کنید"
                    };
                }

                var passwordHasher = new PasswordHasher();
                var hashedPassword = passwordHasher.HashPassword(request.Password);
                var User = new User()
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Password = hashedPassword,
                    IsActive = true,
                };

                var userInRoles = new List<UserInRole>();

                foreach (var item in userInRoles)
                {
                    var role = _context.Roles.Find();
                    userInRoles.Add(new UserInRole()
                    {
                        Role = role,
                        RoleId = role.Id,
                        User = User,
                        UserId = User.Id,

                    });

                    User.UserInRoles = userInRoles;
                    _context.Users.Add(User);
                    _context.SaveChanges();

                }
                return new ResultDto<ResultRegisterUserto>()
                {
                    Data = new ResultRegisterUserto()
                    {
                        UserId = User.Id,
                    },
                    IsSuccess = true,
                    Message = "ثبت نام با وفقیت انجام شد"
                };
            }
            catch
            {
                return new ResultDto<ResultRegisterUserto>()
                {
                    Data = new ResultRegisterUserto()
                    {
                        UserId = 0,
                    },
                    IsSuccess = false,
                    Message = "ثبت نام انجام نشد!"
                };

            }
            }
            //موقتا ریترن نال
            //return null;
        }


    }

    public class RequestRegisterUserDto
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Password { get; set; }
            public string RePassword { get; set; }
            public List<RoleInRegisterUserDto> Roles { get; set; }

        }
        public class RoleInRegisterUserDto
        {
            public string Id { get; set; }
        }

        public class ResultRegisterUserto
        {
            public long UserId { get; set; }
        }
    

