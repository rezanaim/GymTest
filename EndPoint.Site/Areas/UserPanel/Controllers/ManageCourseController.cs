using GymTest.Application.Interfaces.Contexts;
using GymTest.Application.Services.Courses.Query;
using GymTest.Application.Services.Lectures.Command;
using GymTest.Application.Services.Lectures.Query;
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
        private readonly IDeleteLecture _deleteLecture;
        private readonly IEditLecture _editLecture;
        private readonly IGetDataForEditLecture _getDataForEditLecture;

        // متد کمکی
        public ManageCourseController(
            IDataBaseContext context,
            IGetCourseDetail getCourseDetail,
            IAddCourseLecture addCourseLecture,
            IDeleteLecture deleteLecture,
            IEditLecture editLecture,
            IGetDataForEditLecture getDataForEditLecture
            )
        {
            _context = context;
            _getCourseDetail = getCourseDetail;
            _addCourseLecture = addCourseLecture;
            _deleteLecture = deleteLecture;
            _editLecture = editLecture;
            _getDataForEditLecture = getDataForEditLecture;
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
            //AI Claude
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


        [HttpPost]
        public IActionResult DeleteLecture(long LectureId)
        {
            var target = _context.Lectures.Find(LectureId);
            if (target == null)
            {
                return NotFound();
            }

            if (!IsOwner(target.CourseId))
            {
                return NotFound();
            }

            var result = _deleteLecture.Execute(LectureId);

            return Json(result);
        }

        [HttpGet]
        public IActionResult EditLecture(long LectureId)
        {
            var targetLecture = _context.Lectures.Find(LectureId);
            if (targetLecture == null)
            {
                return NotFound();
            }

            if (!IsOwner(targetLecture.CourseId))
            {
                return NotFound();
            }
            
            
            var result = _getDataForEditLecture.Execute(LectureId);
            
            return View(result);
        }

        [HttpPost]
        public IActionResult EditLecture(RequestEditCourseLecture request, IFormFile VideoFile)
        {
            var targetLecture = _context.Lectures.Find(request.LectureId);

            if (targetLecture == null)
            {
                return NotFound();
            }

            if (!IsOwner(targetLecture.CourseId))
            {
                return NotFound();
            }

            // آپلود فایل
            //AI Claude
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
            else
            {
                // اگه فایل جدید نیومد همون قبلی بمونه
                request.ContentPath = targetLecture.ContentPath;
            }

            var result = _editLecture.Execute(request);

            return Json(result);
        }
    }
}
