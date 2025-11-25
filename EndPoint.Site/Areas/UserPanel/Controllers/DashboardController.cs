using GymTest.Application.Services.Users.Command;
using Microsoft.AspNetCore.Http;
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
    public class DashboardController : Controller
    {

        private readonly IAddNewCourse _addNewCourse;


        public DashboardController(IAddNewCourse addNewCourse)
        {
            _addNewCourse = addNewCourse;
        }


        // GET: DashboardController
        public ActionResult Index()
        {

            // نمایش مشخصا کاربر باید اکشن دیفالت پنل کاربری باشه
            return View();
        }

        // GET: DashboardController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DashboardController/Create
        public ActionResult CreateCourse()
        {
            /*
            var sportsList = new List<SelectListItem>
            {

                // ساخت لیستی از ورزش ها  در ویو بگ برای ویویی که قراره جیمینای بسازه
                new SelectListItem { Value = "1", Text = "Football" },
                new SelectListItem { Value = "2", Text = "Pingpong" },
                new SelectListItem { Value = "3", Text = "Chess" },
                new SelectListItem { Value = "4", Text = "Wrestling" },
                new SelectListItem { Value = "5", Text = "Boxing" },
                new SelectListItem { Value = "6", Text = "Ski" }
            };

            // ۲. ارسال این لیست به ویو از طریق ViewBag
            ViewBag.Sports = sportsList;
            */
            return View();
        }

        // POST: DashboardController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateCourse(RequestAddNewCourseDto request)
        {

            long coachId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var addedCourse = _addNewCourse.Execute(request, coachId);

            return Json(addedCourse);
        }

        // GET: DashboardController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DashboardController/Edit/5
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

        // GET: DashboardController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DashboardController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}