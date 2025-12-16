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
    public class MyTeachingController : Controller
    {
        private readonly IGetMyTeachingCourses _getMyTeachingCoursec;

        public MyTeachingController(IGetMyTeachingCourses getMyTeachingCoursec)
        {
            _getMyTeachingCoursec = getMyTeachingCoursec;
        }

        [Authorize]
        public IActionResult Index(RequestGetMyTeachingCourses request)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            request.Id = userId;
            var result = _getMyTeachingCoursec.Execute(request);

            return View(result);
        }
    }
}
