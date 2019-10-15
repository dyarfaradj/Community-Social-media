using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Distribuerade_System_Labb_2.Models;
using Microsoft.AspNetCore.Authorization;
using Distribuerade_System_Labb_2.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Distribuerade_System_Labb_2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
