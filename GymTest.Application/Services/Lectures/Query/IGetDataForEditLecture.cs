using Common;
using GymTest.Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Lectures.Query
{
    public interface IGetDataForEditLecture
    {
        ResultDto<ResultGetDataForEditLecture> Execute(long LectureId);
    }
    public class GetDataForEditLecture : IGetDataForEditLecture
    {
        private readonly IDataBaseContext _context;
        public GetDataForEditLecture(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ResultGetDataForEditLecture> Execute(long LectureId)
        {
            var target = _context.Lectures
                .Include(l=> l.Course)
                .FirstOrDefault(l=> l.Id == LectureId);

            if (target == null)
            {
                return new ResultDto<ResultGetDataForEditLecture>()
                {
                    Data = new ResultGetDataForEditLecture(),
                    IsSuccess = false,
                    Message = "جلسه مورد نظر یافت نشد"
                };
            }

            return new ResultDto<ResultGetDataForEditLecture>()
            {
                Data = new ResultGetDataForEditLecture()
                {
                    LectureId = target.Id,
                    CourseName = target.Course.Title,
                    Title = target.Title,
                    Description = target.Description,
                    OrderCount = target.OrderCount,
                    ContentPath = target.ContentPath,
                    DurationInSecs = target.DurationInSecs,
                    CourseId = target.CourseId
                },
                IsSuccess = true,
                Message = "جلسه مورد نظر آماده ویرایش است"
            };
            
        }
    }

    public class ResultGetDataForEditLecture
    {
        public long CourseId { get; set; }
        public string CourseName{ get; set; }
        public long LectureId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } = "";
        public int? OrderCount { get; set; }
        public string ContentPath { get; set; }
        public int DurationInSecs { get; set; } = 0;
    }
}
