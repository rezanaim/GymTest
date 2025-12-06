using Common;
using GymTest.Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Courses.Command
{
    public interface IEditCourseByAdmin
    {
        ResultDto Execute(RequestEditCourseByAdmin request);
    }

    public class EditCourseByAdmin : IEditCourseByAdmin
    {
        private readonly IDataBaseContext _context;

        public EditCourseByAdmin(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestEditCourseByAdmin request)
        {
            var target = _context.Courses.FirstOrDefault(c=> c.Id == request.CourseId);

            if (target == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "دوره مورد نظر یافت نشد"
                };
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "عنوان دوره را مشخص کنید"
                };
            }

            if (string.IsNullOrWhiteSpace(request.Description))
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "بخش توضیحات را پر کنید"
                };
            }

            var titleCheck = _context.Courses.Any(c => c.Title == request.Title && c.Id != request.CourseId);

            if (titleCheck)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "دوره ای با این نام از قبل موجود است"
                };
            }
            var sportCheck = _context.Sports.Find(request.SportId);

            if (sportCheck == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "رشته مورد نظر شما یافت نشد"
                };
            }

            target.IsActive = request.IsActive;
            target.Title = request.Title;
            target.Description = request.Description;
            target.Price = request.Price;
            target.SportId = request.SportId;


            target.UpdateTime = DateTime.Now;
            _context.SaveChanges();

            var result = new ResultDto()
            {
                IsSuccess = true,
                Message = "دوره مورد نظر با موفقیت ویرایش شد"
            };


            return result;
        }
    }

    public class RequestEditCourseByAdmin
    {
        public long CourseId { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public long SportId { get; set; }
    }
}
