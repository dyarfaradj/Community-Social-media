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
using BusinessLogic.MessageLogic;
using Microsoft.AspNetCore.Identity;

namespace Distribuerade_System_Labb_2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly MessageLogic messageLogic = new MessageLogic();
        private readonly Distribuerade_System_Labb_2Context _context;
        private readonly UserManager<Distribuerade_System_Labb_2User> _userManager;

        public HomeController(Distribuerade_System_Labb_2Context context, UserManager<Distribuerade_System_Labb_2User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
                int SumDeleted = await messageLogic.HowManyMessageDeleted(currentUser.Id);
                int SumRead = await messageLogic.HowManyMessagesRead(currentUser.Id);
                int SumUnRead = await messageLogic.HowManyMessagesUnRead(currentUser.Id);
                int TotMessages = await messageLogic.HowManyMessages(currentUser.Id);
                ViewBag.NoOfDeletedMessages = SumDeleted;
                ViewBag.NoOfMessages = TotMessages;
                ViewBag.NoOfReadMessages = SumRead;
                ViewBag.NoOfUnReadMessages = SumUnRead;
            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
