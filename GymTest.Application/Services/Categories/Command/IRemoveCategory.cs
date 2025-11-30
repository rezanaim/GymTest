using Common;
using GymTest.Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Categories.Command
{
    public interface IRemoveCategory
    {
        ResultDto Execute(RequestRemoveCategory request);
    }

    public class RemoveCategory : IRemoveCategory
    {

        private readonly IDataBaseContext _context;

        public RemoveCategory(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestRemoveCategory request)
        {
            var target = _context.Categories.Find(request.CategoryId);

            if (target == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "کتگوری مورد نظر یافت نشد"
                };
            }
            //زیرشاخه ها و اسپورتز  ای کالکشن هستن
            //و کالکشن ها هرگز نال نمیشن حتی اگه یه لیست خالی باشن
            // واسه همین از !=null استفاده نمیکنیم توی ایف ها

            if (target.SubCategories.Any())
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = 
                    "فقط کتگوری هایی که هیچ زیرشاخه ای ندارند قابل حذف هستند"
                };
            }
            if (target.Sports.Any())
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "کتگوری ای که شامل ورزش هایی باشد، قابل حذف نیستند"
                };
            }

            target.RemoveTime = DateTime.Now;
            target.IsRemoved = true;
            _context.SaveChanges();

            var result = new ResultDto()
            {
                IsSuccess = true,
                Message = "کتگوری مورد نظر با موفقیت حذف شد"
            };

            return result;
        }
    }


    public class RequestRemoveCategory
    {
        public long CategoryId { get; set; }
    }
}
