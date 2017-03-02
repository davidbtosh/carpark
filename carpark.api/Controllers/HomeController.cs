using carpark.api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace carpark.api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View(new UserUI());
        }

        [HttpGet]
        public ActionResult Calculate()
        {
            ViewBag.Title = "Home Page";

            return View(new UserUI());
        }
    }
}
