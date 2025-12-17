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
    public interface IGetCourseDetail
    {
        public ResultDto<ResultGetCourseDetail> Execute(RequestGetCourseDetail request);

    }

    public class GetCourseDetail : IGetCourseDetail
    {
        private readonly IDataBaseContext _Context;
        public GetCourseDetail(IDataBaseContext context)
        {
            _Context = context;
        }
        public ResultDto<ResultGetCourseDetail> Execute(RequestGetCourseDetail request)
        {
            const int pageSize = 10;

            var target = _Context.Courses
                .Include(p=> p.Coach)
                .Include(p=> p.Sport)
                .FirstOrDefault(p=> p.Id == request.CourseId);

            if (target == null)
            {
                return new ResultDto<ResultGetCourseDetail>()
                {
                    Data = new ResultGetCourseDetail()
                    {

                    },
                    IsSuccess = false,
                    Message = "دوره مورد نظر یافت نشد"
                };
            }
            
            var subsCount = _Context.UserInCourses.Where(p => p.CourseId == request.CourseId).Count();
            var totalLecturesList = _Context.Lectures.Where(p => p.CourseId == request.CourseId);
            var totalLecturesCount = totalLecturesList.Count();
            // توتال لکچر با توتال رو متفاوته
            // چون توتال رو بسته به سرچ کی ممکنه کمتر از توتال لکچرز بشه

            var lectureList = _Context.Lectures.Where(p => p.CourseId == request.CourseId);

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                lectureList = lectureList.Where(p =>
                    p.Title.Contains(request.SearchKey) ||
                    p.Description.Contains(request.SearchKey)
                    );
            }
            var totalRows = lectureList.Count(); 

                var lectureListShow = lectureList
                .Select(p => new LectureDto()
                {
                    Title = p.Title,
                    Description = p.Description,
                    OrderCount = p.OrderCount,
                    Content = p.Content

                })
            
                .Skip((request.PageNumber - 1 )* pageSize)
                .Take(pageSize)
                .ToList();

            int CourseDuration = 0;
            
            foreach (Lecture i in totalLecturesList)
            {
                CourseDuration = CourseDuration + i.DurationInSecs;
            }


            return new ResultDto<ResultGetCourseDetail>()
            {

                

                Data = new ResultGetCourseDetail()
                {
                    Title = target.Title,
                    Description = target.Description,
                    Price = target.Price,
                    CouchName = (target.Coach.FirstName + " " + target.Coach.LastName),
                    SportName = target.Sport.Name,
                    IsActive = target.IsActive,
                    Id = target.Id,

                    Subscribes = subsCount,
                    LecturesList = lectureListShow,
                    LectureCount = totalLecturesCount,

                    TotalRows = totalRows,
                    DurationInSecs = CourseDuration
                },
                IsSuccess = true,
                Message = $"جزئیات دوره {target.Title} "
            };
            


        }
    }

    public class LectureDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int OrderCount { get; set; }
        public string Description { get; set; }
    }

        public class ResultGetCourseDetail
    {
        public int TotalRows { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CouchName { get; set; }
        public string SportName { get; set; }
        public bool IsActive { get; set; }
        public int LectureCount { get; set; }
        public int DurationInSecs { get; set; }
        public long Id { get; set; }

        public List<LectureDto> LecturesList { get; set; }
        public int Subscribes { get; set; }
    }

    public class RequestGetCourseDetail
    {
        public long CourseId { get; set; }
        public int PageNumber { get; set; } = 1;
        public string SearchKey { get; set; } = "";
    }
}
