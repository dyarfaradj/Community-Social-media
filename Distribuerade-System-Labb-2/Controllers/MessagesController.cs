using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Distribuerade_System_Labb_2.Models;
using Microsoft.AspNetCore.Identity;
using Distribuerade_System_Labb_2.Areas.Identity.Data;

namespace Distribuerade_System_Labb_2.Controllers
{
    public class MessagesController : Controller
    {
        private readonly Distribuerade_System_Labb_2Context _context;
        private readonly UserManager<Distribuerade_System_Labb_2User> _userManager;

        public MessagesController(Distribuerade_System_Labb_2Context context, UserManager<Distribuerade_System_Labb_2User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Messages
        public async Task<IActionResult> Index()
        {
            Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
            List<Message> messages = await _context.Messages.Where(message =>( message.User.Equals(currentUser) || message.ReceiverId.Equals(currentUser.Id))).ToListAsync();

            return View(messages);
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            if(!IsItemByCurrentUser(message))
            {
                return Unauthorized();
            }

            return View(message);
        }

        // GET: Messages/Create
        public IActionResult Create()
        {
            var users = _context.Users.ToList();
            List<SelectListItem> usernameList = new List<SelectListItem>();
            string currentUserId =  _userManager.GetUserId(User);

            foreach (Distribuerade_System_Labb_2User u in users)
            {
                if(u.Id != currentUserId) //Tar bort personen själv i listan
                 usernameList.Add(new SelectListItem { Value = u.Id, Text = u.UserName });
            }

            ViewBag.Users = new SelectList(usernameList, "Value", "Text");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Body,ReceiverId")] Message message)
        {
            Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                message.Deleted = false;
                message.Read = false;
                message.SentDate = DateTime.Now;
                message.User = currentUser;
                _context.Add(message);
                await _context.SaveChangesAsync();
                //ViewBag.Message = "You clicked NO!";
                TempData["ConfirmationMessage"] = "Meddelande nummer " + 3 +" avsänt till " + GetUserById(message.ReceiverId).UserName;
                return RedirectToAction("Create");
            }
            return View(message);
        }

        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body,Read,Deleted,SentDate,ReceiverId")] Message message)
        {
            if (id != message.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == id);
           
            if (message == null)
            {
                return NotFound();
            }

            message.Deleted = true;
            try
            {
                _context.Update(message);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(message.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));            
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (!IsItemByCurrentUser(message))
            {
                return Unauthorized();
            }

            message.Deleted = true;
            try
            {
                _context.Update(message);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(message.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }

        private Distribuerade_System_Labb_2User GetUserById(String id)
        {
            return _context.Users.Find(id);
        }

        private bool IsItemByCurrentUser(Message message)
        {
            string currentUserId = _userManager.GetUserId(User);
            return message.User.Id.Equals(currentUserId);
        }
    }
}
