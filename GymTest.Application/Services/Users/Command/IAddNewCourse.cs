using Common;
using GymTest.Application.Interfaces.Contexts;
using GymTest.Domain.Entities.CourseNmore;
using GymTest.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Users.Command
{
    public interface IAddNewCourse
    {
        ResultDto<ResultAddNewCourseDto> Execute(RequestAddNewCourseDto request, long coachId);
    }

    public class AddNewCourse : IAddNewCourse
    {
        private readonly IDataBaseContext _context;

        public AddNewCourse(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ResultAddNewCourseDto> Execute(RequestAddNewCourseDto request, long coachId)
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

            //اگه طرف رول کوچ نداشت باید بهش اضافه کنیم

            //var theCoach = _context.Users.Find(coachId);



            //IQueryable<User>
            IQueryable<User> queryUser = _context.Users
                .Include(u => u.UserInRoles)
                .ThenInclude(u => u.Role);

            var theCoach = queryUser.FirstOrDefault(u => u.Id == coachId);

            if (theCoach == null)
            {
                return new ResultDto<ResultAddNewCourseDto>()
                {
                    Data = new ResultAddNewCourseDto()
                    {
                        CourseId = 0,
                    },
                    IsSuccess = false,
                    Message = "سازنده دوره یافت نشد. اول ثبت نام کنید"
                };
            }

            var temp = theCoach.UserInRoles;

            var roles = temp.Select(r => r.RoleId).ToList();

            var coachRole = _context.Roles.FirstOrDefault(r => r.Name == "Coach");
            if (coachRole == null)
            {
                return new ResultDto<ResultAddNewCourseDto>()
                {
                    Data = new ResultAddNewCourseDto()
                    {
                        CourseId = 0,
                    },
                    IsSuccess = false,
                    Message = "رول Coach ایجاد نشده است"
                };
            }
            var coachRoleId = coachRole.Id;
            if (!roles.Contains(coachRoleId))
            {
                _context.UserInRoles.Add(new UserInRole { UserId = coachId, RoleId = coachRoleId });
            }

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
        public long CoachId { get; set; }
        public long SportId { get; set; }


    }

    public class ResultAddNewCourseDto
    {
        public long CourseId { get; set; }
    }


}