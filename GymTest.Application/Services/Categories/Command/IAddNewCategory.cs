using Common;
using GymTest.Application.Interfaces.Contexts;
using GymTest.Domain.Entities.CourseNmore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Categories.Command
{
    public interface IAddNewCategory
    {
        ResultDto<ResultAddNewCategory> Execute(RequestAddNewCategory request);

    }

    public class AddNewCategory : IAddNewCategory
    {

        private readonly IDataBaseContext _context;

        public AddNewCategory(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultAddNewCategory> Execute(RequestAddNewCategory request)
        {

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new ResultDto<ResultAddNewCategory>()
                {
                    Data = new ResultAddNewCategory()
                    {

                    },                    
                    IsSuccess =false,
                    Message = "لطفا نام کتگوری را وارد کنید"
                };
            }

            if (_context.Categories.Any(c => c.Name == request.Name))
            {
                return new ResultDto<ResultAddNewCategory> 
                {
                    Data = new ResultAddNewCategory()
                    {

                    },
                    IsSuccess = false, Message = "نام دسته‌بندی تکراری است." 

                };
            }

            var newCategory = new Category()
            {
                Name = request.Name,
                ParentId = request.ParentId
            };

            _context.Categories.Add(newCategory);
            _context.SaveChanges();

            var result = new ResultDto<ResultAddNewCategory>()
            {
                Data = new ResultAddNewCategory()
                {
                    CategoryId = newCategory.Id
                },
                IsSuccess = true,
                Message = "کتگوری با موفقیت افزوده شد"
            };
            

            return result;
        }
    }

    public class RequestAddNewCategory
    {
        public string Name { get; set; }
        public long? ParentId { get; set; }

    }

    public class ResultAddNewCategory
    {
        public long CategoryId { get; set; }
    }
}
