using Common;
using GymTest.Application.Interfaces.Contexts;
using GymTest.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Users.Command
{
    public interface IDeleteUser
    {
        public ResultDto Execute(RequestDeleteDto request);
    }

    public class DeleteUser : IDeleteUser
    {
        private readonly IDataBaseContext _context;
        public DeleteUser(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestDeleteDto request)
        {
            var user = _context.Users.Find(request.UserId);

            if (user == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = $"کاربری با شناسه مورد نظر( {request.UserId} ) یافت نشد"
                };
            }

            user.RemoveTime = DateTime.Now;
            user.IsRemoved = true;
            user.IsActive = false;
            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = $"کاربر مورد نظر({user.FirstName} {user.LastName}) با شناسه ({user.Id}) با موفقیت حذف شد"
            };
        }

    }


    public class RequestDeleteDto
    {
        public long UserId { get; set; }
    }

    /*
    public class ResultDeleteDto
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    */
}
