using carpark.api.Models;
using carpark.api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace carpark.api.Controllers
{
    public class HomeController : Controller
    {
        private IRatesCalculator _ratesCalculator;

        public HomeController(IRatesCalculator ratesCalculator)
        {
            _ratesCalculator = ratesCalculator;            
        }

        public HomeController()
        {
            _ratesCalculator = new RatesCalculator();           
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View(new UserUI());
        }

        [HttpPost]
        public ActionResult Calculate(UserUI ui)
        {
            ViewBag.Title = "Home Page";

            ui.AssembleDatesTimes();
            _ratesCalculator.FilePath = Server.MapPath("~/App_Data/");
            ui.RateDisplay = _ratesCalculator.CalculateRate(ui);

            return View("Index", ui);
        }
    }
}
