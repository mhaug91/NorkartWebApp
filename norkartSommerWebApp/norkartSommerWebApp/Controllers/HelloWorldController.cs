using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace norkartSommerWebApp.Controllers
{
    public class HelloWorldController : Controller
    {
        // GET: HelloWorld
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Welcome(string name, int numTimes = 1)
        {
            ViewBag.Message = "Hello testing" + name;
            ViewBag.NumTimes = numTimes;
            return View();
        }

        public ActionResult AirQualityReport()
        {
            return View();
        }

        public String Default()
        {
            return "default ppage";
        }
    }
}