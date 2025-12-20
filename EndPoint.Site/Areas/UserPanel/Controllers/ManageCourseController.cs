using GymTest.Application.Interfaces.Contexts;
using GymTest.Application.Services.Courses.Query;
using GymTest.Application.Services.Lectures.Command;
using Microsoft.AspNetCore.Http; // برای اپلود ویدیو
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO; // برای اپلود ویدیو
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    public class ManageCourseController : Controller
    {
        private readonly IDataBaseContext _context;
        private readonly IGetCourseDetail _getCourseDetail;
        private readonly IAddCourseLecture _addCourseLecture;


        // متد کمکی
        public ManageCourseController(
            IDataBaseContext context,
            IGetCourseDetail getCourseDetail,
            IAddCourseLecture addCourseLecture
            )
        {
            _context = context;
            _getCourseDetail = getCourseDetail;
            _addCourseLecture = addCourseLecture;
        }

        private bool IsOwner(long courseId)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var targetCourse = _context.Courses.Find(courseId);


            return ((targetCourse != null) && (targetCourse.CoachId == userId));

        }
        public IActionResult Index(RequestGetCourseDetail request)
        {
            if (!IsOwner(request.CourseId))
            {
                return NotFound();
            }
            var result = _getCourseDetail.Execute(request);


            return View(result);
        }

        [HttpPost]
        public IActionResult AddLecture(RequestAddCourseLecture request, IFormFile VideoFile)
        {
            if (!IsOwner(request.CourseId))
            {
                return NotFound();
            }
            // cloude


            // آپلود فایل
            if (VideoFile != null && VideoFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "lectures");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(VideoFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    VideoFile.CopyTo(stream);
                }

                request.ContentPath = "/uploads/lectures/" + fileName;
            }


            var result = _addCourseLecture.Execute(request);

            return Json(result);
        }
    }
}
