using GymTest.Application.Services.Sports.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SportsController : Controller
    {
        private readonly IGetSports _getSports;
        public SportsController(IGetSports getSports)
        {
            _getSports = getSports;
        }




        // GET: SportsController
        
        public ActionResult Index(string searchKey = "" , int pageNumber = 1)
        {

            var request = new RequestGetSportsDto()
            {
                PageNumber = pageNumber,
                SearchKey = searchKey
            };
            var showSports = _getSports.Execute(request);
            // نمایش مشخصا کاربر باید اکشن دیفالت پنل کاربری باشه
            return View(showSports);

        }


        // GET: SportsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SportsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SportsController/Create
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

        // GET: SportsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SportsController/Edit/5
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

        // GET: SportsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SportsController/Delete/5
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
