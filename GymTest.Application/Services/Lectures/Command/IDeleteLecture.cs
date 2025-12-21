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
    public interface IDeleteLecture
    {
        ResultDto Execute(long LectureId);
    }

    public class DeleteLecture : IDeleteLecture
    {
        private readonly IDataBaseContext _context;

        public DeleteLecture(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(long LectureId)
        {
            var target = _context.Lectures
                
                .Find(LectureId);
            if (target == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "جلسه مورد نظر یافت نشد"
                };
            }

            var efectedLectures = _context.Lectures.Where(l => l.CourseId == target.CourseId && l.OrderCount > target.OrderCount).ToList();
            foreach (Lecture l in efectedLectures)
            {
                l.OrderCount = l.OrderCount - 1;
            }

            target.IsRemoved = true;
            target.RemoveTime = DateTime.Now;
            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "جلسه مورد نظر با موفقیت حذف شد"
            };
        }
    }
}
