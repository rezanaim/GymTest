using GymTest.Application.Interfaces.Contexts;
using GymTest.Common;
using GymTest.Domain.Entities.CourseNmore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Categories.Query
{
    public interface IGetCategories
    {
        ResultGetCategoriesDto Execure(RequestGetCategoriesDto request);
    }

    public class GetCategories : IGetCategories
    {
        private readonly IDataBaseContext _context;

        public GetCategories(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultGetCategoriesDto Execure(RequestGetCategoriesDto request)
        {
            const int pageSize = 5;
            IQueryable<Category> query = _context.Categories;

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                query = query.Where(p =>
                p.Name.Contains(request.SearchKey)
                );
            }

            var showList = query.ToPaged(request.PageNumber, pageSize, out int rowsCount)
                .Select(p => new GetCategoryDto()
                {
                    Name = p.Name,
                    Id = p.Id
                }).ToList();


            var result = new ResultGetCategoriesDto()
            {
                Categories = showList,
                TotalRows = rowsCount

            };


            return (result);
        }
    }

    public class RequestGetCategoriesDto
    {
        public string SearchKey { get; set; }
        public int PageNumber { get; set; }
    }

    public class GetCategoryDto
    {
        public string Name { get; set; }
        public long Id { get; set; }
        //public string ParentName { get; set; }

    }

    public class ResultGetCategoriesDto
    {
        public List<GetCategoryDto> Categories { get; set; }
        public int TotalRows { get; set; }

    }
}
