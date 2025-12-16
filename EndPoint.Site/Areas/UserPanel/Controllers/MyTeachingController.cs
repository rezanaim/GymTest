using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    public class MyTeachingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
