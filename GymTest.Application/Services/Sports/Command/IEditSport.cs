using Common;
using GymTest.Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Sports.Command
{
    public interface IEditSport
    {
        ResultDto Execute(RequestEditSport request);
    }

    public class EditSport : IEditSport
    {
        private readonly IDataBaseContext _context;

        public EditSport(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestEditSport request)
        {
            var target = _context.Sports.Find(request.SportId);
            if (target == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "ورزش مورد نظر یافت نشد"
                };
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "لطفا نام ورزش را وارد کنید"
                };
            }

            if (string.IsNullOrWhiteSpace(request.Description))
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "لطفا توضیحات ورزش را وارد کنید"
                };
            }

            var categoryCheck = _context.Categories.Find(request.CategoryId);

            if (categoryCheck == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "لطفا کتگوری ورزش ورد نظر را مشخص کنید"
                };
            }


            var nameCheck = _context.Sports.Any(
                s => s.Name == request.Name && s.Id != request.SportId);

            if (nameCheck == true)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "ورزشی با این نام موجود است"
                };
            }

            target.Name = request.Name;
            target.Description = request.Description;
            target.CategoryId = request.CategoryId;
            target.UpdateTime = DateTime.Now;

            _context.SaveChanges();

            var result = new ResultDto()
            {
                IsSuccess = true,
                Message = "ورزش مورد نظر با موفقیت ویرایش شد"
            };

            return result;
        }
    }

    public class RequestEditSport
    {
        public long SportId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }

    }
}
