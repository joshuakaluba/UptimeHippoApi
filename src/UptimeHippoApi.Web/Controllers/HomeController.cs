using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using UptimeHippoApi.Data.Models.Authentication;
using UptimeHippoApi.Data.Models.Domain.ViewModels;

namespace UptimeHippoApi.Web.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        public HomeController(UserManager<ApplicationUser> userManager,
            ILogger<HomeController> logger)
            : base(userManager, logger)
        {
        }

        //Home/Index
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        //Home/Health
        [HttpGet]
        public IActionResult Health()
        {
            return Ok();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}