using GymTest.Application.Services.Sports.Query;
using GymTest.Application.Services.Users.Command;
using GymTest.Application.Services.Users.Query.GetUserDetails;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IGetSports _getSports;
        private readonly IGetUserDetails _getUserDetails;

        public DashboardController(
            IAddNewCourse addNewCourse,
            IGetSports getSports,
            IGetUserDetails getUserDetails
            )
        {
            _addNewCourse = addNewCourse;
            _getSports = getSports;
            _getUserDetails = getUserDetails;
        }


        // GET: DashboardController
        public ActionResult Index()
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var details = _getUserDetails.Execute(userId);
            //ای دی رو به جای ورودی اکشن و خوندن از  یو ار ال
            // ک خطرناکه از نظر امنیتی. از کلیم یوزر میخونیم
 
            return View(details.Data);
        }

        // GET: DashboardController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DashboardController/Create
        [HttpGet]
        public ActionResult CreateCourse()
        {
            var showSports = _getSports.Execute(new RequestGetSportsDto()
            {
                PageNumber = 1,
                SearchKey = ""
            });

            ViewBag.SportsList = new SelectList(showSports.SportsList, "Id", "Name");

            return View();
        }

        // POST: DashboardController/Create
        [HttpPost]
        [Authorize]
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