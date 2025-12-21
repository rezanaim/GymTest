using Common;
using GymTest.Application.Interfaces.Contexts;
using GymTest.Domain.Entities.CourseNmore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Lectures.Command
{
    public interface IEditLecture
    {
        public ResultDto Execute(RequestEditCourseLecture request);
    }

    public class EditLecture : IEditLecture
    {
        private readonly IDataBaseContext _context;
        public EditLecture(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestEditCourseLecture request)
        {
            var target = _context.Lectures.Find(request.LectureId);
            var targetCourse = _context.Courses.Find(request.CourseId);


            if (targetCourse == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "دوره مورد نظر یافت نشد"
                };
            }

            if (target == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "جلسه مورد نظر یافت نشد"
                };
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "لطفا عنوان جلسه را وارد کنید"
                };
            }

            if (string.IsNullOrWhiteSpace(request.ContentPath))
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "لطفا محتوا را اپلود کنید"
                };
            }
            var orderCounts = _context.Lectures.Where(l => l.CourseId == request.CourseId).ToList();

            var lastOrderCount = orderCounts
                .Max(l => (int?)l.OrderCount) ?? 0;
            if (!request.OrderCount.HasValue)  
            {
                request.OrderCount = lastOrderCount + 1;
            }
            else if(target.OrderCount != request.OrderCount)
            {
                foreach (Lecture l in orderCounts)
                {
                    if (l.OrderCount > target.OrderCount)
                    {
                        l.OrderCount--;
                    }
                }

                foreach (Lecture l in orderCounts)
                {
                    if (l.OrderCount >= request.OrderCount)
                    {
                        l.OrderCount++;
                    }
                }
            }

            target.Title = request.Title;
            target.Description = request.Description;
            target.OrderCount = request.OrderCount.Value;
            target.ContentPath = request.ContentPath;
            target.DurationInSecs = request.DurationInSecs;
            target.UpdateTime = DateTime.Now;

            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "جلسه مورد نظر با موفقیت ویرایش شد"
            };
        }
    }

    public class RequestEditCourseLecture
    {
        public long LectureId { get; set; }
        public long CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } = "";
        public int? OrderCount { get; set; }
        public string ContentPath { get; set; }
        public int DurationInSecs { get; set; } = 0;
    }
}
