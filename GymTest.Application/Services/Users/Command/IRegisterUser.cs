using Common;
using GymTest.Application.Interfaces.Contexts;
using GymTest.Common;
using GymTest.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static GymTest.Application.Services.Users.Command.RegisterUser;

namespace GymTest.Application.Services.Users.Command
{
    public interface IRegisterUser
    {
        ResultDto<ResultRegisterUserDto> Execute(RequestRegisterUserDto request);
    }
    public class RegisterUser : IRegisterUser
    {

        private readonly IDataBaseContext _context;
        public RegisterUser(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultRegisterUserDto> Execute(RequestRegisterUserDto request)
        {
            try
            {



                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "لطفا ایمیل خود را وارد کنید"
                    };
                }

                if (string.IsNullOrWhiteSpace(request.FirstName))
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "لطفا نام خود را وارد کنید"
                    };
                }

                if (string.IsNullOrWhiteSpace(request.LastName))
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "لطفا نام خانوادگی خود را وارد کنید"
                    };
                }

                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "لطفا یک پسورد انتخاب کنید"
                    };
                }

                if (request.Password != request.RePassword)
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
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
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "لطفا یک ایمیل معتبر وارد کنید"
                    };
                }

                if (_context.Users.Any(p => p.Email.Equals(request.Email)))
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "قبلا با ای ایمیل ثبت نام شده است "
                    };
                }

                var passwordHasher = new PasswordHasher();
                var hashedPassword = passwordHasher.HashPassword(request.Password);
                var user = new User()
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Password = hashedPassword,
                    IsActive = true,
                };

                _context.Users.Add(user);
                _context.SaveChanges();



                // فریچ مرگبار - این حلقه لیستش خالیه و هیچوقت سیو چنج و.. نمیشه
                // ضمن اینکه اگه خالی نبود اصلا منطقشم درست نیست و نباید اینجوری باشه

                /*
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

                }*/
                return new ResultDto<ResultRegisterUserDto>()
                {
                    Data = new ResultRegisterUserDto()
                    {
                        UserId = user.Id,
                    },
                    IsSuccess = true,
                    Message = "ثبت نام با وفقیت انجام شد"
                };
            }
            catch
            {
                return new ResultDto<ResultRegisterUserDto>()
                {
                    Data = new ResultRegisterUserDto()
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
            [MinLength(5, ErrorMessage = "پسورد باید حداقل 5 کاراکتر باشد")]
            public string Password { get; set; }
            public string RePassword { get; set; }
            public List<RoleInRegisterUserDto> Roles { get; set; }

        }
        public class RoleInRegisterUserDto
        {
            public long Id { get; set; }
        }

        public class ResultRegisterUserDto
        {
            public long UserId { get; set; }
        }
    

