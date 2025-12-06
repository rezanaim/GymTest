using Common;
using GymTest.Application.Interfaces.Contexts;
using GymTest.Domain.Entities.CourseNmore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Sports.Command
{
    public interface IAddNewSport
    {
        ResultDto<ResultAddNewSportDto> Execute(RequestAddNewSportDto request);
    }


    public class AddNewSport : IAddNewSport
    {
        private readonly IDataBaseContext _context;

        public AddNewSport(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ResultAddNewSportDto> Execute(RequestAddNewSportDto request)
        {

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new ResultDto<ResultAddNewSportDto>()
                {
                    Data = new ResultAddNewSportDto()
                    {

                    },

                    IsSuccess = false,
                    Message = "لطفا نام ورزش را وارد کنید"
                };
            }

            var categoryCheck = _context.Categories.Find(request.CategoryId);

            if (categoryCheck == null)
            {
                return new ResultDto<ResultAddNewSportDto>()
                {
                    Data = new ResultAddNewSportDto()
                    {

                    },

                    IsSuccess = false,
                    Message = "لطفا کتگوری ورزش را وارد کنید"
                };
            }


            var nameCheck = _context.Sports.Any(s => s.Name == request.Name);

            if (nameCheck == true)
            {
                return new ResultDto<ResultAddNewSportDto>()
                {
                    Data = new ResultAddNewSportDto()
                    {

                    },

                    IsSuccess = false,
                    Message = "این ورزش موجود است"
                };
            }

            var newSport = new Sport()
            {
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId
            };
            _context.Sports.Add(newSport);
            _context.SaveChanges();

            var result = new ResultDto<ResultAddNewSportDto>()
            {
                Data = new ResultAddNewSportDto()
                {
                    SportId = newSport.Id
                },

                IsSuccess = true,
                Message = "ورزش مورد نظر با موفقیت اضافه شد"
            };

            return result;
        }
    }




    public class RequestAddNewSportDto
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }

    }

    public class ResultAddNewSportDto
    {
        public long SportId { get; set; }
    }
}
