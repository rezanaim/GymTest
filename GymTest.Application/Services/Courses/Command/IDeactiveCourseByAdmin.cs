using Common;
using GymTest.Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Courses.Command
{
    public interface IDeactiveCourseByAdmin
    {
        ResultDto Execute(RequestDeactiveCourseByAdmin request);
    }

    public class DeactiveCourseByAdmin : IDeactiveCourseByAdmin
    {
        private readonly IDataBaseContext _context;

        public DeactiveCourseByAdmin(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestDeactiveCourseByAdmin request)
        {
            var target = _context.Courses.Find(request.CourseId);

            if (target == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "دوره مورد نظر یافت نشد"
                };
            }

            target.IsActive = false;
            target.UpdateTime = DateTime.Now;
            _context.SaveChanges();

            var result = new ResultDto()
            {
                IsSuccess = true,
                Message = "دوره مورد نظر با موفقیت غیر فعال شد"
            };


            return result;
        }
    }

    public class RequestDeactiveCourseByAdmin
    {
        public long CourseId { get; set; }

    }
}
