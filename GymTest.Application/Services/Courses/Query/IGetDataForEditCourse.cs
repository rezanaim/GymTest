using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GymTest.Application.Interfaces.Contexts;

namespace GymTest.Application.Services.Courses.Query
{
    public interface IGetDataForEditCourse
    {
        ResultDto<CourseDataDto> Execute(RequestDataForEditCourseByAdmin request);
    }

    public class GetDataForEditCourse : IGetDataForEditCourse
    {

        private readonly IDataBaseContext _context;

        public GetDataForEditCourse(IDataBaseContext context)
        {

            _context = context;
        }

        public ResultDto<CourseDataDto> Execute(RequestDataForEditCourseByAdmin request)
        {

            var target = _context.Courses.Find(request.CourseId);


            if (target == null)
            {
                return new ResultDto<CourseDataDto>()
                {
                    Data = new CourseDataDto()
                    {

                    },
                    IsSuccess = false,
                    Message = "دوره مورد نظر یافت نشد"
                };
            }


            var result = new CourseDataDto()
            {
                IsActive = target.IsActive,
                Title = target.Title,
                Description = target.Description,
                Price = target.Price,
                SportId = target.SportId

            };

            var finalResult = new ResultDto<CourseDataDto>()
            {
                Data = result,
                IsSuccess = true,
                Message = "اطلاعات کلی دوره"
            };

            return finalResult;
        }
    }

    public class RequestDataForEditCourseByAdmin
    {
        public  long CourseId { get; set; }
    }

    public class CourseDataDto
    {
        public long CourseId { get; set; }

        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public long SportId { get; set; }

    }
}
