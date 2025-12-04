using GymTest.Application.Services.Courses.Command;
using GymTest.Application.Services.Courses.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{   
    [Area("Admin")]
    public class CoursesController : Controller
    {
        private readonly IGetCourses _getCourses;
        private readonly IEditCourseByAdmin _deactiveCourseByAdmin;
        private readonly IDeleteCourseByAdmin _deleteCourseByAdmin;

        public CoursesController(
            IGetCourses getCourses,
            IEditCourseByAdmin deactiveCourseByAdmin,
            IDeleteCourseByAdmin deleteCourseByAdmin

            )
        {
            _getCourses = getCourses;
            _deactiveCourseByAdmin = deactiveCourseByAdmin;
            _deleteCourseByAdmin = deleteCourseByAdmin;
        }

        // GET: CourseController
        public ActionResult Index(string searchKey = "", int pageNumber = 1, bool isActive = true)
        {
            var reques = new RequestGetCoursesDto()
            {
                IsActive = isActive,
                SearchKey = searchKey,
                PageNumber = pageNumber
            };

            var result = _getCourses.Execute(reques);
            return View(result);
        }

        // GET: CourseController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CourseController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CourseController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CourseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        /*
        // GET: CourseController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        */
        // POST: CourseController/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var request = new RequestDeleteCourseByAdmin()
            {
                CourseId = id,
            };

            var result = _deleteCourseByAdmin.Execute(request);
            return Json(result);
            
        }
    }
}
