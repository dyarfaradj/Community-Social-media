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
using Microsoft.AspNetCore.Authorization;
using BusinessLogic.MessageLogic;
using BusinessLogic.UserLogic;
using AutoMapper;
using DataAcces.DataContext;
using DataAcces.Entities;

namespace Distribuerade_System_Labb_2.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly MessageLogic messageLogic = new MessageLogic();
        private readonly UserLogic userLogic = new UserLogic();
        private readonly DatabaseContext _context;
        private readonly UserManager<User> _userManager;

        public MessagesController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // GET: Messages
        //public async Task<IActionResult> MessageOfUser(string id)
        //{
        //    if (id.Equals(null) || id.Equals(""))
        //    {
        //        return NotFound();
        //    }

        //    UserViewModel currentUser = await _userManager.GetUserAsync(User);
        //    List<Message> messages = await _context.Messages.Where(message =>
        //    (message.ReceiverId.Equals(currentUser.Id) && message.Deleted.Equals(false) && message.User.Id.Equals(id))).ToListAsync();
        //    var messages2 = _context.Messages.ToList();
        //    int SumDeleted = 0;
        //    int SumRead = 0;
        //    int TotMessages = 0;
        //    foreach (Message m in messages2)
        //    {
        //        if(IsMessageForMe(m))
        //        {
        //            TotMessages++;
        //            if (m.Deleted != false)
        //                SumDeleted++;
        //            if ( m.Read != false && m.Deleted == false)
        //                SumRead++;
        //        }
        //    }

        //    ViewBag.NoOfDeletedMessages = SumDeleted;
        //    ViewBag.NoOfMessages = TotMessages;
        //    ViewBag.NoOfReadMessages = SumRead;
        //    return View(messages);
        //}
        public async Task<IActionResult> Index()
        {

            User currentUser = await _userManager.GetUserAsync(User);
            List<MessageViewModel> messageList= new List<MessageViewModel>();
            List<User> users = new List<User>();

            var messages = await messageLogic.GetAllMessages();
            var usersList = await GetAllUser();

            if (messages.Count>0)
            {
                foreach(var m in messages)
                {
                    if((m.ReceiverId.Equals(currentUser.Id)) && m.Deleted.Equals(false))
                    {
                        foreach (User u in usersList)
                        {
                            if (m.SenderId.Equals(u.Id) && !users.Contains(u))
                            {
                                users.Add(u);

                            }
                        }
                    }
                }
            }
            return View(users);
        }

        //public async Task<IActionResult> Index2()
        //{
        //    Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
        //    List<Message> messages = await _context.Messages.Where(message =>
        //    ((message.User.Equals(currentUser) || message.ReceiverId.Equals(currentUser.Id))
        //    && message.Deleted.Equals(false))).ToListAsync();
        //    int TotMessages = 0;
        //    List<Distribuerade_System_Labb_2User> Us = await _context.Users.Where(u => (!u.Id.Equals(currentUser.Id))).ToListAsync();
        //    List<Distribuerade_System_Labb_2User> users = new List<Distribuerade_System_Labb_2User>();

        //    foreach (Message m in messages)
        //    {
        //        foreach (Distribuerade_System_Labb_2User u in Us)
        //        {
        //            if (m.User.Id.Equals(u.Id) && !users.Contains(u))
        //            {
        //                users.Add(u);
        //                TotMessages++;
        //            }
        //        }
        //    }
        //    MessageViewModel currentMessage = new MessageViewModel
        //    {
        //        Title = m.Title,
        //        Body = m.Body,
        //        Read = m.Read,
        //        Deleted = m.Deleted,
        //        SentDate = m.SentDate,
        //        ReceiverId = m.ReceiverId,
        //        SenderId = m.SenderId
        //    };
        //    messageList.Add(currentMessage);


        //    ViewBag.NoOfMessages = TotMessages;
        //    return View(users);
        //}

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

            message.Read = true;
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
            return View(message);
        }

        // GET: Messages/Create
        public async Task<IActionResult> Create()
        {
            var usersList =  await GetAllUser();
            List<SelectListItem> usernameList = new List<SelectListItem>();
            string currentUserId =  _userManager.GetUserId(User);

           foreach (User u in usersList)
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
        public async Task<IActionResult> Create([Bind("Title,Body,ReceiverId")] MessageViewModel message)
        {
            User currentUser = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                bool result = await messageLogic.CreateNewMessage(message.Title, message.Body, message.ReceiverId, currentUser.Id);
               // TempData["ConfirmationMessage"] = "Meddelande nummer " + currentUser.Messages.Last().Id + " avsänt till " 
                //    + GetUserById(message.ReceiverId).UserName + ", " + DateTime.Now.ToString("H:mm yyyy-MM-dd");
                return RedirectToAction("Create");
            }
            return View(message);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create2([Bind("Title,Body,ReceiverId")] Message message)
        //{
        //    UserViewModel currentUser = await _userManager.GetUserAsync(User);
        //    if (ModelState.IsValid)
        //    {
        //        message.Deleted = false;
        //        message.Read = false;
        //        message.SentDate = DateTime.Now;
        //        message.User = currentUser;
        //        _context.Add(message);
        //        await _context.SaveChangesAsync();
        //        TempData["ConfirmationMessage"] = "Meddelande nummer " + currentUser.Messages.Last().Id + " avsänt till "
        //            + GetUserById(message.ReceiverId).UserName + ", " + DateTime.Now.ToString("H:mm yyyy-MM-dd");
        //        return RedirectToAction("Create");
        //    }
        //    return View(message);
        //}


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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body,Read,Deleted,SentDate,ReceiverId")] MessageViewModel message)
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
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //        var messages =  _context.Messages.ToList();
        //        int SumDeleted = 0;
        //        foreach (Message message in messages)
        //        {
        //            if (message.Deleted != false) //Tar bort personen själv i listan
        //                SumDeleted++;
        //        }
        //        ViewBag.NoOfDeletedMessages = SumDeleted;
        //        return RedirectToAction(nameof(Index));
            
        //}

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }

        //private Distribuerade_System_Labb_2User GetUserById(String id)
        //{
        //    return _context.Users.Find(id);
        //}

        //private bool IsMessageForMe(Message message)
        //{
        //    string currentUserId = _userManager.GetUserId(User);
        //    return message.ReceiverId.Equals(currentUserId);
        //}

        private async Task<List<User>> GetAllUser()
        {
            List<User> users = new List<User>();
            var usersList = await userLogic.GetAllUsers();

            if (usersList.Count > 0)
            {
                foreach (var m in usersList)
                {
                    User currenUser = new User
                    {
                        Id = m.Id,
                        UserName = m.UserName,
                        LastLoginDate = m.LastLoginDate,
                        LoginPerMonth = m.LoginPerMonth,
                        Email = m.Email,
                    };
                    users.Add(currenUser);
                }
            }
            return users;
        }

        //private bool IsItemByCurrentUser(Message message)
        //{
        //    string currentUserId = _userManager.GetUserId(User);
        //    return message.User.Id.Equals(currentUserId);
        //}
    }
}
