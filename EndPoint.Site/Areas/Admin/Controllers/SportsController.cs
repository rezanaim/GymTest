using GymTest.Application.Services.Categories.Query;
using GymTest.Application.Services.Sports.Command;
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
    public class SportsController : Controller
    {
        private readonly IGetSports _getSports;
        private readonly IAddNewSport _addNewSport;
        private readonly IGetCategories _getCategories;
        private readonly IRemoveSport _removeSport;
        public SportsController(
            
            IGetSports getSports,
            IAddNewSport addNewSport,
            IGetCategories getCategories,
            IRemoveSport removeSport
            )
        {
            _getSports = getSports;
            _addNewSport = addNewSport;
            _getCategories = getCategories;
            _removeSport = removeSport;
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
        [HttpGet]
        public ActionResult Create( )
        {
            var request = new RequestGetCategoriesDto()
            {
                SearchKey = "",
                PageNumber = 1
            };
            var allCategoriesList = _getCategories.Execute(request).Categories;

            ViewBag.CategoriesList = new SelectList(allCategoriesList, "Id", "Name");

            return View();
        }

        // POST: SportsController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(RequestAddNewSportDto request)
        {


            var result = _addNewSport.Execute(request);

            return Json(result);
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


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            var result = _removeSport.Execute(id);

            return Json(result);
        }
    }
}
