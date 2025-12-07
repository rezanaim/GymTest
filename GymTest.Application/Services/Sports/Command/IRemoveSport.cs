using Common;
using GymTest.Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Application.Services.Sports.Command
{
    public interface IRemoveSport
    {

        ResultDto Execute(long sportId);
    }

    public class RemoveSport : IRemoveSport
    {
        private readonly IDataBaseContext _context;
        public RemoveSport(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(long sportId)
        {
            var target = _context.Sports.Find(sportId);

            if (target == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "ورزش مورد نظر یافت نشد"
                };
            }

            var hasCourse = _context.Courses.Any(c=> c.SportId == sportId);
            if (hasCourse)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "اول دوره هایی که از این ورزش هستند را پاک کنید"
                };
            }

            target.IsRemoved = true;
            _context.SaveChanges();


            return new ResultDto()
            {
                IsSuccess = true,
                Message = "ورزش مورد نظر با موفقیت حذف شد"
            };
            
        }
    }
}
