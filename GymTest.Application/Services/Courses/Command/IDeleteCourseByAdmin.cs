using Common;
using GymTest.Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Courses.Command
{
    public interface IDeleteCourseByAdmin
    {

        ResultDto Execute(RequestDeleteCourseByAdmin request);
    }

    public class DeleteCourseByAdmin : IDeleteCourseByAdmin
    {
        private readonly IDataBaseContext _context;
        public DeleteCourseByAdmin(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(RequestDeleteCourseByAdmin request)
        {
            var target = _context.Courses.Find(request.CourseId);

            if (target == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "دوره مورد نظر پیدا نشد"
                };
            }
            target.IsActive = false;
            target.IsRemoved = true;
            target.RemoveTime = DateTime.Now;
            _context.SaveChanges();

            var result = new ResultDto()
            {
                IsSuccess = true,
                Message = "دوره مورد نظر با موفقیت حذف شد"
            };
            return result;
        }
    }

    public class RequestDeleteCourseByAdmin
    {
        public long CourseId { get; set; }

    }
}
