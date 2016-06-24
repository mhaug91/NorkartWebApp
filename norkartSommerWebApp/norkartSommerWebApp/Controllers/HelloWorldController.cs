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

        public String Welcome()
        {
            return "hi world";
        }

        public String Default()
        {
            return "default ppage";
        }
    }
}