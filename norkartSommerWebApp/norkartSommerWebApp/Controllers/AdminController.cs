﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using norkartSommerWebApp.Models;
using System.Threading.Tasks;

namespace norkartSommerWebApp.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Welcome(string name, int numTimes = 1)
        {
            ViewBag.Message = "Hello " + name;
            ViewBag.NumTimes = numTimes;
            return View();
        }

        public String Default()
        {
            return "default page";
        }
        [HttpPost]
        public async Task<ActionResult> IotMessage(string message)
        {
            
            System.Diagnostics.Debug.WriteLine("SUBMIT VALUE: " + message);
            switch (message)
            {
                
                case "AirQuality":
                    // delegate sending to another controller action
                    await IotHub.Main("startAirQuality");
                    System.Diagnostics.Debug.WriteLine("SomeText");
                    return (View("Index"));
                case "Restart":
                    System.Diagnostics.Debug.WriteLine("SomeText2");
                    // call another action to perform the cancellation
                    await IotHub.Main("startAirQuality");
                    return (View("Index"));
                default:
                    System.Diagnostics.Debug.WriteLine("SomeText3");
                    // If they've submitted the form without a submitButton, 
                    // just return the view again.
                    return (View("Index"));
            }
        }
    }
}
