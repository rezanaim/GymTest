using Common;
using GymTest.Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Users.Query.GetUserDetails
{
    public interface IGetUserDetails
    {
        public ResultDto<UserDto> Execute(long Id);
    }

    public class GetUserDetails : IGetUserDetails
    {
        private readonly IDataBaseContext _context;
        public GetUserDetails(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<UserDto> Execute(long Id)
        {

            //var target = _context.Users.Find(Id);

            var target = _context.Users
                .Include(u => u.UserInRoles)
                .ThenInclude(u => u.Role)
                .FirstOrDefault(u => u.Id == Id);

            //if(userQuery.Any())
            if (target == null)
            {
                return new ResultDto<UserDto>()
                {
                    Data = new UserDto()
                    {

                    },
                    IsSuccess = false,
                    Message = "کاربر مورد نظر یافت نشد"
                };
            }

            var userDtail = new UserDto()
            {
                UserId = target.Id,
                FirstName = target.FirstName,
                LastName = target.LastName,
                Email = target.Email,
                RegisterTime = target.RegisterTime,
                IsActive = target.IsActive,
                Roles = target.UserInRoles.Select(r => r.Role.Name).ToList()

            };

            return new ResultDto<UserDto>()
            {
                Data = userDtail,
                IsSuccess = true,
                Message = "مشخصات اکانت شما"


            };

        }
    }


    public class UserDto
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RegisterTime { get; set; }
        public List<string> Roles { get; set; }
        public bool IsActive { get; set; }

    }
}
