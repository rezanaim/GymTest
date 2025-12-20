using GymTest.Application.Interfaces.Contexts;
using GymTest.Application.Services.Courses.Command;
using GymTest.Application.Services.Courses.Query;
using GymTest.Application.Services.Lectures.Command;
using GymTest.Application.Services.Sports.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    public class MyTeachingController : Controller
    {
        private readonly IDataBaseContext _context;
        private readonly IGetMyTeachingCourses _getMyTeachingCoursec;
        private readonly IEditCourse _editCourse;
        private readonly IGetDataForEditCourse _getDataForEditCourse;
        private readonly IGetSports _getSports;

        public MyTeachingController(
            IGetMyTeachingCourses getMyTeachingCoursec,
            IEditCourse editCourse,
            IDataBaseContext context,
            IGetDataForEditCourse getDataForEditCourse,
            IGetSports getSports
            )
        {
            _getMyTeachingCoursec = getMyTeachingCoursec;
            _editCourse = editCourse;
            _context = context;
            _getDataForEditCourse = getDataForEditCourse;
            _getSports = getSports;
        }

        [Authorize]
        public IActionResult Index(RequestGetMyTeachingCourses request)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            request.Id = userId;
            var result = _getMyTeachingCoursec.Execute(request);

            return View(result);
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {

            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var target = _context.Courses.Find(id);

            if (target == null)
            {
                return NotFound();
            }
            // این تارگت = نال توی خود سرویس های ادیت گت و پست چک میشه
            // ولی اینجا قبل ایف پایینی چک میکنیم
            // چون تو ایف پایینی قبل اجرای سرویس. اگه تارگت نال باشه کرش میکنه
            if (target.CoachId != userId)
            {
                return Unauthorized();
            }
            var requestSports = new RequestGetSportsDto()
            {
                SearchKey = "",
                PageNumber = 1
            };
            var sportList = _getSports.Execute(requestSports);
            ViewBag.SportsList = new SelectList(sportList.SportsList, "Id", "Name");

            var request = new RequestDataForEditCourse()
            {
                CourseId = id,
            };
            var result = _getDataForEditCourse.Execute(request);



            return View(result);
        }

        [HttpPost]
        public IActionResult Edit(RequestEditCourse request)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var target = _context.Courses.Find(request.CourseId);

            if (target == null)
            {
                return NotFound();
            }
            if (target.CoachId != userId)
            {
                return Unauthorized();
            }

            var result = _editCourse.Execute(request);

            return Json(result);
        }




    }
}
