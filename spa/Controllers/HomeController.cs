using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SUR.Util1;

namespace spa.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var groups = ActiveDirectory.GetGroupNames(User.Identity.Name);

            return View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
