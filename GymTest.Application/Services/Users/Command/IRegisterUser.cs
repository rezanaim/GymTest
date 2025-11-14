using Common;
using GymTest.Application.Interfaces.Contexts;
using GymTest.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            //موقتا  ریترن نال
            return null;
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
            public string UserId { get; set; }
        }
    }
}
