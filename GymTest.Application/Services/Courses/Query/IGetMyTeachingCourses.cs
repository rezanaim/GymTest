using Common;
using GymTest.Application.Interfaces.Contexts;
using GymTest.Domain.Entities.CourseNmore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Courses.Query
{
    public interface IGetMyTeachingCourses
    {
        ResultDto<ResultGetMyTeachingCourses> Execute(RequestGetMyTeachingCourses request);
    }


    public class GetMyTeachingCourses : IGetMyTeachingCourses
    {
        private readonly IDataBaseContext _context;
        public GetMyTeachingCourses(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ResultGetMyTeachingCourses> Execute(RequestGetMyTeachingCourses request)
        {
            const int pageSize = 5;

            IQueryable<Course> UICQ = _context.Courses
                .Include(p => p.Coach)
                .Include(p => p.Sport)
                .Where(p => p.CoachId == request.Id);



            var target = _context.Users.Find(request.Id);

            if (target == null)
            {
                return new ResultDto<ResultGetMyTeachingCourses>()
                {
                    Data = new ResultGetMyTeachingCourses()
                    {

                    },
                    IsSuccess = false,
                    Message = "حساب کاربری یافت نشد"
                };
            }

            if (request.IsActive.HasValue)
            {
                UICQ = UICQ.Where(p => p.IsActive == request.IsActive);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                UICQ = UICQ.Where(p =>

                p.Title.Contains(request.SearchKey) ||
                p.Description.Contains(request.SearchKey) ||
                p.Sport.Name.Contains(request.SearchKey) ||
                (p.Coach.FirstName + " " + p.Coach.LastName)
                .Contains(request.SearchKey)

                );
            }

            var targetList = UICQ
                .OrderByDescending(p=> p.Id)
                .Skip((request.PageNumber - 1)* pageSize)
                .Take(pageSize)
                .Select(p => new CourseDto()
                {
                    Title = p.Title,
                    Description = p.Description,
                    SportName = p.Sport.Name,
                    Price = p.Price,
                    CoachName = (p.Coach.FirstName + " " + p.Coach.LastName),
                    DurationInMins = p.DurationInSecs,
                    LectureCount = p.LectureCount,
                    IsActive = p.IsActive,
                    Id = p.Id
                }).ToList();

            var totalRows = UICQ.Count();

            return new ResultDto<ResultGetMyTeachingCourses>()
            {
                Data = new ResultGetMyTeachingCourses()
                {
                    TotalRows = totalRows,
                    MyTeachingCourses = targetList
                },
                IsSuccess = true,
                Message = "لیست دوره هایی که تدریس کرده ام"
            };
            
        }
    }

    public class RequestGetMyTeachingCourses
    {
        public long Id { get; set; }
        public string SearchKey { get; set; }
        public int PageNumber { get; set; } = 1;
        public bool? IsActive { get; set; }
    }

    public class ResultGetMyTeachingCourses
    {
        public int TotalRows { get; set; }
        public List<CourseDto> MyTeachingCourses { get; set; }
    }
}
