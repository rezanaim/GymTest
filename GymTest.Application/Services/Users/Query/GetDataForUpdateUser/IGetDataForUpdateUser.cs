using Common;
using GymTest.Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Users.Query.GetDataForUpdateUser
{
    public interface IGetDataForUpdateUser
    {
        ResultDto<UserDto> Execute(long Id);

    }

    public class GetDataForUpdateUser : IGetDataForUpdateUser
    {

        private readonly IDataBaseContext _context;

        public GetDataForUpdateUser(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<UserDto> Execute(long Id)
        {
            var user = _context.Users.Find(Id);

            if (user == null)
            {
                return new ResultDto<UserDto>()
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "کاربر مورد نظر یافت نشد"
                };
            }
            var result = new ResultDto<UserDto>()
            {
                Data = new UserDto()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    IsActive = user.IsActive,
                },
                IsSuccess = true,
                Message = "مشخصات کاربر مورد نظر:"
            };
            return (result);



            
        }
    }

    public class UserDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
