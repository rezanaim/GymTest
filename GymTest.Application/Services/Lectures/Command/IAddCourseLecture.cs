using Common;
using GymTest.Application.Interfaces.Contexts;
using GymTest.Domain.Entities.CourseNmore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Lectures.Command
{
    public interface IAddCourseLecture
    {
        public ResultDto<ResultAddCourseLecture> Execute(RequestAddCourseLecture request);
    }

    public class AddCourseLecture : IAddCourseLecture
    {
        private readonly IDataBaseContext _context;

        public AddCourseLecture(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ResultAddCourseLecture> Execute(RequestAddCourseLecture request)
        {
            var targetCourse = _context.Courses.Find(request.CourseId);
            /*
             * var targetCourse = _context.Courses
                .Include(p=> p.Lectures)
                .FirstOrDefault(p => p.Id == request.CourseId);
            */
            if (targetCourse == null)
            {
                return new ResultDto<ResultAddCourseLecture>()
                {
                    Data = new ResultAddCourseLecture()
                    {

                    },
                    IsSuccess = false,
                    Message = "دوره مورد نظر یافت نشد"
                };
            }
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return new ResultDto<ResultAddCourseLecture>()
                {
                    Data = new ResultAddCourseLecture()
                    {

                    },
                    IsSuccess = false,
                    Message = "لطفا عنوان جلسه را وارد کنید"
                };
            }

            if (string.IsNullOrWhiteSpace(request.ContentPath))
            {
                return new ResultDto<ResultAddCourseLecture>()
                {
                    Data = new ResultAddCourseLecture()
                    {

                    },
                    IsSuccess = false,
                    Message = "لطفا محتوا را اپلود کنید"
                };
            }

            var orderCounts = _context.Lectures.Where(p => p.CourseId == targetCourse.Id);
            var lastOrderCount = orderCounts
                .Max(l => (int?)l.OrderCount) ?? 0;

            var orderCountsList = orderCounts.ToList();

            var theOrderCount = 0;
            if (request.OrderCount.HasValue)
            {
                theOrderCount = request.OrderCount.Value;
                foreach (Lecture i in orderCountsList)
                {
                    if (i.OrderCount >= request.OrderCount)
                    {
                        i.OrderCount++;
                    }
                }
            }
            else
            {
                theOrderCount = lastOrderCount + 1;
            }

            var lecture = new Lecture()
            {
                Title = request.Title,
                Description = request.Description,
                ContentPath = request.ContentPath,
                CourseId = request.CourseId,
                OrderCount = theOrderCount,
                DurationInSecs = request.DurationInSecs
                
            };

            _context.Lectures.Add(lecture);
            _context.SaveChanges();

            return new ResultDto<ResultAddCourseLecture>()
            {
                Data = new ResultAddCourseLecture()
                {
                    LectureId = lecture.Id
                },
                IsSuccess = true,
                Message = "جلسه مورد نظر با موفقیت ثبت شد"
            };
        }
    }

    public class RequestAddCourseLecture
    {
        public long CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } = "";
        public int? OrderCount { get; set; }
        public string ContentPath { get; set; }
        public int DurationInSecs { get; set; } = 0;
        //public DateTime InsertTime { get; set; } = DateTime.Now;


    }
    public class ResultAddCourseLecture
    {
        public long LectureId { get; set; }
    }
}
