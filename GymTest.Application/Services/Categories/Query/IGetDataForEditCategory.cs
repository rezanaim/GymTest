using Common;
using GymTest.Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Categories.Query
{
    public interface IGetDataForEditCategory
    {
        ResultDto<CategoryDto> Execute(RequestDataForEditCategory request);
    }

    public class GetDataForEditCategory : IGetDataForEditCategory
    {
        private readonly IDataBaseContext _context;

        public GetDataForEditCategory(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<CategoryDto> Execute(RequestDataForEditCategory request)
        {
            var target = _context.Categories.Find(request.CategoryId);

            if (target == null)
            {
                return new ResultDto<CategoryDto>()
                {
                    Data = new CategoryDto(),
                    IsSuccess = false,
                    Message = "اطلاعات کتگوری مورد نظر یافت نشد"
                };
            }
            var data = new CategoryDto()
            {
                CategoryId = target.Id,
                CategoryName = target.Name,
                ParentId = target.ParentId
            };

            var result = new ResultDto<CategoryDto>()
            {
                Data = data,
                IsSuccess = true,
                Message = "مشخصات کتگوری مورد نظر"
            };

            return result;
        }
    }

    public class RequestDataForEditCategory
    {

        public long CategoryId { get; set; }
    }

    public class CategoryDto
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public long? ParentId { get; set; }
    }
}
