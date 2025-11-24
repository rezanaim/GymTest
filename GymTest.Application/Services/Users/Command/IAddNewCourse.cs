using Common;
using GymTest.Application.Interfaces.Contexts;
using GymTest.Domain.Entities.CourseNmore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Users.Command
{
    public interface IAddNewCourse
    {
        ResultDto<ResultAddNewCourseDto> Execute(RequestAddNewCourseDto request);
    }

    public class AddNewCourse : IAddNewCourse
    {
        private readonly IDataBaseContext _context;

        public AddNewCourse(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ResultAddNewCourseDto> Execute(RequestAddNewCourseDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return new ResultDto<ResultAddNewCourseDto>()
                {
                    Data = new ResultAddNewCourseDto()
                    {
                        CourseId = 0,
                    },
                    IsSuccess = false,
                    Message = "لطفا عنوان دوره را وارد کنید"
                    
                };
            }

            if (string.IsNullOrWhiteSpace(request.Description))
            {
                return new ResultDto<ResultAddNewCourseDto>()
                {
                    Data = new ResultAddNewCourseDto()
                    {
                        CourseId = 0,
                    },
                    IsSuccess = false,
                    Message = "لطفا توضیحات دوره را وارد کنید"

                };
            }

            var newCourse = new Course()
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.price,
                CoachId = request.CoachId,
                SportId = request.SportId
            };

            _context.Courses.Add(newCourse);
            _context.SaveChanges();

            var result = new ResultDto<ResultAddNewCourseDto>()
            {
                Data = new ResultAddNewCourseDto()
                {
                    CourseId = newCourse.Id
                },
                IsSuccess = true,
                Message = $"دوره{newCourse.Title} با موفقیت ایجاد شد"
                
            };

            return (result);
        }
    }



    public class RequestAddNewCourseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal price { get; set; }
        public long CoachId {get;set;}
        public long SportId { get; set; }
        

    }

    public class ResultAddNewCourseDto
    {
        public long CourseId { get; set; }
    }
}
