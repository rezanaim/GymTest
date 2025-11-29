using GymTest.Application.Interfaces.Contexts;
using GymTest.Domain.Entities.CourseNmore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;




namespace GymTest.Application.Services.Courses.Query
{
    public interface IGetCourses
    {

        ResultGetCoursesDto Execute(RequestGetCoursesDto request);
    }

    public class GetCourses : IGetCourses
    {
        private readonly IDataBaseContext _context;

        public GetCourses(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultGetCoursesDto Execute(RequestGetCoursesDto request)
        {
            const int pageSize = 5;
            IQueryable<Course> CourseQuery = _context.Courses
            .Include(p => p.Coach)
            .Include(p => p.Sport);


            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                CourseQuery = CourseQuery.Where(p =>
                p.Title.Contains(request.SearchKey) ||
                p.Description.Contains(request.SearchKey) ||
                p.Sport.Name.Contains(request.SearchKey) ||
                p.Coach.FirstName.Contains(request.SearchKey) ||
                (p.Coach.FirstName + " " + p.Coach.LastName)
                .Contains(request.SearchKey)

            ); }

            //var courseList = CourseQuery.ToPaged()


            int totlaRows = CourseQuery.Count();

            var courseList = CourseQuery
                .OrderByDescending(p => p.Id)
                .Skip((request.PageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new CourseDto()
                {
                    Title = p.Title,
                    Description = p.Description,
                    price = p.Price,
                    DurationInMins = p.DurationInMins,
                    LectureCount = p.LectureCount,
                    CoachName = (p.Coach.FirstName + " " + p.Coach.LastName),
                    SportName = p.Sport.Name

                }).ToList();
                

            throw new NotImplementedException();
        }
    }



    public class RequestGetCoursesDto
    {
        public string SearchKey { get; set; }
        public int PageNumber { get; set; }
    }

    public class ResultGetCoursesDto
    {
        public List<Course> CourseList { get; set; }
        public int TotalRows { get; set; }
    }

    public class CourseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal price { get; set; }
        public int DurationInMins { get; set; }
        public int LectureCount { get; set; }
        public string CoachName { get; set; }
        public string SportName { get; set; }


    }
}
