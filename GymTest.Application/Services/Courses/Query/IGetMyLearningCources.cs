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

namespace GymTest.Application.Services.Courses.Query
{
    public interface IGetMyLearningCources
    {
        ResultDto<ResultGetMyLearningCourses> Execute(RequestGetMyLearningCourses request);
    }


    public class GetMyLearningCources : IGetMyLearningCources
    {

        private readonly IDataBaseContext _context;
        public GetMyLearningCources(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ResultGetMyLearningCourses> Execute(RequestGetMyLearningCourses request)
        {

            var target = _context.Users.Find(request.Id);
            if (target == null)
            {
                return new ResultDto<ResultGetMyLearningCourses>()
                {
                    Data = new ResultGetMyLearningCourses()
                    {

                    },
                    IsSuccess = false,
                    Message = "حساب کاربری یافت نشد"
                };
            }
            /*
            IQueryable<UserInCourse> UserInCourseQ = _context.UserInCourses
                .Include(u => u.User)
                .Include(u=> u.Course)
                .Where(u=> u.UserId == request.Id)
                .Select(p=> new CourseDto()
                {

                })
                ;
            */

            //.FirstOrDefault(u => u.Id == request.Id);

            IQueryable<UserInCourse> UserInCourseQ = _context.UserInCourses
                .Include(u => u.User)
                .Include(u => u.Course)
                    .ThenInclude(u=> u.Sport)
                .Include(u=> u.Course)
                    .ThenInclude(u=> u.Coach)
                .Where(u => u.UserId == request.Id);


            if (request.IsActive.HasValue)
            {
                UserInCourseQ = UserInCourseQ.Where(c => c.Course.IsActive == request.IsActive);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                UserInCourseQ = UserInCourseQ.Where(c =>

                c.Course.Title.Contains(request.SearchKey) ||
                c.Course.Description.Contains(request.SearchKey) ||
                c.Course.Sport.Name.Contains(request.SearchKey) ||
                (c.Course.Coach.FirstName + " " + c.Course.Coach.LastName)
                .Contains(request.SearchKey)
                );
            };


            const int pageSize = 5;

            int totalRows = UserInCourseQ.Count();

            var targetlist = UserInCourseQ
                .OrderByDescending(p=> p.Id)
                .Skip((request.PageNumber - 1)* pageSize )
                .Take(pageSize)
                .Select(p =>
            new CourseDto()
            {
                Title = p.Course.Title,
                Description = p.Course.Description,
                IsActive = p.Course.IsActive,
                Id = p.Course.Id,
                Price = p.Course.Price,
                DurationInMins = p.Course.DurationInMins,
                LectureCount = p.Course.LectureCount,
                CoachName = (p.Course.Coach.FirstName + " " + p.Course.Coach.LastName),
                SportName = p.Course.Sport.Name
            }).ToList();

            return new ResultDto<ResultGetMyLearningCourses>()
            {
                Data = new ResultGetMyLearningCourses()
                {
                    TotalRow = totalRows,
                    MyLearningCourses = targetlist
                },
                IsSuccess = true,
                Message = "لیست دوره هایی که شرکت کرده ام"
            };
            
        }
    }


    public class ResultGetMyLearningCourses
    {
        public int TotalRow { get; set; }
        public List<CourseDto> MyLearningCourses { get; set; }
        // کورس دی تی او چون توی سرویس گت کورس بود و توی همین پوشه هستش
        // نیم اسپیسشون یکیه و از همون قبلی استفاده میکنم
    }

    public class RequestGetMyLearningCourses
    {
        public long Id { get; set; }
        public string SearchKey { get; set; }
        public int PageNumber { get; set; } = 1;
        public bool? IsActive { get; set; }
    }
}
