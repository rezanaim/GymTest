using GymTest.Application.Services.Courses.Command;
using GymTest.Application.Services.Courses.Query;
using GymTest.Application.Services.Sports.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IEditCourse _editCourseByAdmin;
        private readonly IDeleteCourseByAdmin _deleteCourseByAdmin;
        private readonly IGetDataForEditCourse _getDataForEditCourse;
        private readonly IGetSports _getSports;

        public CoursesController(
            IGetCourses getCourses,
            IEditCourse editCourseByAdmin,
            IDeleteCourseByAdmin deleteCourseByAdmin,
            IGetDataForEditCourse getDataForEditCourse,
            IGetSports getSports



            )
        {
            _getCourses = getCourses;
            _editCourseByAdmin = editCourseByAdmin;
            _deleteCourseByAdmin = deleteCourseByAdmin;
            _getDataForEditCourse = getDataForEditCourse;
            _getSports = getSports;
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
        public ActionResult Edit(long id)
        {
            var request = new RequestDataForEditCourse()
            {
                CourseId = id
            };
            var courseResult = _getDataForEditCourse.Execute(request);
            if (courseResult.IsSuccess == false)
            {
                return NotFound();
            }

            var courseData = courseResult.Data;

            var requestSports = new RequestGetSportsDto()
            {
                PageNumber = 1,
                SearchKey = ""
            };
            var allSports = _getSports.Execute(requestSports).SportsList;
            ViewBag.SportsList = new SelectList(allSports, "Id", "Name", courseData.SportId);
 
            return View(courseData);
        }

        // POST: CourseController/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(RequestEditCourse request)
        {
            var result = _editCourseByAdmin.Execute(request);

            return Json(result);

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
