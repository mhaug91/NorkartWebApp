﻿using System;
using System.Web.Mvc;

namespace norkartSommerWebApp.Controllers
{
    public class AirQualityController : Controller
    {
        // GET: AirQuality
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