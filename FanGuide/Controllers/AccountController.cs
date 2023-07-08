using FanGuide.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanGuide.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            var adminEmail = _configuration["AdminEmail"];
            var adminPassword = _configuration["AdminPassword"];
            if (model.Email != adminEmail || model.Password != adminPassword)
            {
                ModelState.AddModelError("Email", "Invalid email or password");
            }

            if (ModelState.IsValid)
            {
                ControllerContext.HttpContext.Session.SetString("Name", model.Email);
                return Redirect("../Home/Index");
            }

            return View();
        }

        public IActionResult Logout()
        {
            ControllerContext.HttpContext.Session.Remove("Name");
            return Redirect("../Home/Index");
        }
    }
}
