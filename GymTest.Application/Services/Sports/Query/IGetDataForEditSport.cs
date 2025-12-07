using Common;
using GymTest.Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Sports.Query
{
    public interface IGetDataForEditSport
    {
        ResultDto<ResultGetDataForEditSportDto> Execute(long SportId);
    }


    public class GetDataForEditSport : IGetDataForEditSport
    {
        private readonly IDataBaseContext _context;

        public GetDataForEditSport(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ResultGetDataForEditSportDto> Execute(long SportId)
        {

            var target = _context.Sports.Find(SportId);
            if (target == null)
            {
                return new ResultDto<ResultGetDataForEditSportDto>()
                {
                    Data = new ResultGetDataForEditSportDto()
                    {

                    },
                    IsSuccess = false,
                    Message = "ورزش مورد نظر یافت نشد"
                };
            }

            return new ResultDto<ResultGetDataForEditSportDto>()
            {
                Data = new ResultGetDataForEditSportDto()
                {
                    SportId = target.Id,
                    Name = target.Name,
                    Description = target.Description,
                    CategoryId = target.CategoryId
                },
                IsSuccess = true,
                Message = "جزئیات ورزش مورد نظر"
            };
        }
    }

    public class ResultGetDataForEditSportDto
    {
        public long SportId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }
    }
}
