using GymTest.Application.Services.Courses.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    public class MyCoursesController : Controller
    {
        private readonly IGetMyLearningCources _getMyLearningCources;

        public MyCoursesController(IGetMyLearningCources getMyLearningCources)
        {
            _getMyLearningCources = getMyLearningCources;
        }

        [Authorize]
        public IActionResult Index(RequestGetMyLearningCourses request)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            request.Id = userId;
            var result = _getMyLearningCources.Execute(request);

            return View(result);
        }
    }
}
