using GymTest.Application.Interfaces.Contexts;
using GymTest.Domain.Entities.CourseNmore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Sports.Query
{
    public interface IGetSports
    {
        ResultGetSportsDto Execute(RequestGetSportsDto request);
    }

    public class GetSports : IGetSports
    {
        private readonly IDataBaseContext _context;

        public GetSports(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultGetSportsDto Execute(RequestGetSportsDto request)
        {
            const int pageSize = 25;
            IQueryable<Sport> sportQuery = _context.Sports
                .Include(s => s.Category);

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                sportQuery = sportQuery.Where(s =>
                s.Name.Contains(request.SearchKey) ||
                s.Description.Contains(request.SearchKey) ||
                s.Category.Name.Contains(request.SearchKey)
                );
            }

            int totalRows = sportQuery.Count();

            var showList = sportQuery.OrderByDescending(s => s.Id)
                .Skip((request.PageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new SportDto()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category.Name

                }).ToList();


            return new ResultGetSportsDto()
            {
                SportsList = showList,
                TotalRows = totalRows
            };
        }
    }

    public class RequestGetSportsDto
    {
        public string SearchKey { get; set; }
        public int PageNumber { get; set; }
        //public bool? IsActive { get; set; }

    }

    public class ResultGetSportsDto
    {
        public List<SportDto> SportsList { get; set; }
        public int TotalRows { get; set; }
    }

    public class SportDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }
}
