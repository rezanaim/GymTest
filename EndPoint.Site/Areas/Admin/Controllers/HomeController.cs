using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestController : Controller
    {
        // URL: /Admin/Test
        public IActionResult Index()
        {
            return Content("Hello from Admin Test Controller!");
        }

        // URL: /Admin/Test/Check
        public IActionResult Check()
        {
            return Json(new { Status = "OK", Message = "Test controller is working." });
        }
    }
}