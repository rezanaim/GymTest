using Common;
using GymTest.Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GymTest.Application.Services.Categories.Command
{
    public interface IEditCategory
    {
        ResultDto Execute(RequestEditCategoryDto request);

    }


    public class EditCategory : IEditCategory
    {
        private readonly IDataBaseContext _context;

        public EditCategory(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestEditCategoryDto request)
        {

            var target = _context.Categories.Find(request.CategoryId);
            //.Include(c => c.ParentCategory.Id)
            //.FirstOrDefault(c => c.Id == request.CategoryId);

            if (target == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "کتگوری مورد نظر یافت نشد"
                };
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "لطفا نام جدید کتگوری را وارد کنید"
                };
            }

            var nameCheck = _context.Categories.Any(c => c.Name.Equals(request.Name));
            if (nameCheck)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "کتگوری ای با این نام موجود است، نام دیگری وارد کنید"
                };
            }




            target.Name = request.Name;
            target.ParentId = request.ParentCategoryId;
            target.UpdateTime = DateTime.Now;
            _context.SaveChanges();


            return new ResultDto()
            {
                IsSuccess = true,
                Message = "کتگوری با موفقیت ویرایش شد"
            };
        }
    }

    public class RequestEditCategoryDto
    {
        public long CategoryId { get; set; }
        public string Name { get; set; }
        public long? ParentCategoryId { get; set; }
    }
}
