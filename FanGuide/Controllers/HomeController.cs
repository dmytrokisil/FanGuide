using FanGuide.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FanGuide.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            return View();
        }
    }
}
