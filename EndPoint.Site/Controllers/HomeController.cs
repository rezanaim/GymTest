using EndPoint.Site.Models;
using GymTest.Application.Services.Courses.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGetCourses _getCourses;


        public HomeController(ILogger<HomeController> logger, IGetCourses getCourses)
        {
            _logger = logger;
            _getCourses = getCourses;
        }

        public IActionResult Index(string searchKey = "", int pageNumber = 1)
        {
            var request = new RequestGetCoursesDto()
            {
                IsActive = true,
                SearchKey = searchKey,
                PageNumber = pageNumber
            };

            var result = _getCourses.Execute(request);

            return View(result);
        }

        public IActionResult CourseDetail()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
