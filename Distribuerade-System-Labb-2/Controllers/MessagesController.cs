﻿using System;
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
using System.Diagnostics;

namespace Distribuerade_System_Labb_2.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly MessageLogic messageLogic = new MessageLogic();
        private readonly Distribuerade_System_Labb_2Context _context;
        private readonly UserManager<Distribuerade_System_Labb_2User> _userManager;

        public MessagesController(Distribuerade_System_Labb_2Context context, UserManager<Distribuerade_System_Labb_2User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Messages
        public async Task<IActionResult> MessageOfUser(string id)
        {
            if (id.Equals(null) || id.Equals(""))
            {
                return NotFound();
            }

            Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
            List<MessageViewModel> messages = await GetAllMessages();
            int SumDeleted = 0;
            int SumRead = 0;
            int TotMessages = 0;
            foreach (MessageViewModel m in messages)
            {
                if(IsMessageForMe(m))
                {
                    TotMessages++;
                    if (m.Deleted != false)
                        SumDeleted++;
                    if ( m.Read != false && m.Deleted == false)
                        SumRead++;
                }
            }

            ViewBag.NoOfDeletedMessages = SumDeleted;
            ViewBag.NoOfMessages = TotMessages;
            ViewBag.NoOfReadMessages = SumRead;
            return View(messages);
        }
        public async Task<IActionResult> Index()
        {
            List<MessageViewModel> messageList = await GetAllMessages();
            Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
            List<Distribuerade_System_Labb_2User> Us = await _context.Users.Where(u => (!u.Id.Equals(currentUser.Id))).ToListAsync();
            List<Distribuerade_System_Labb_2User> users = new List<Distribuerade_System_Labb_2User>();
            int TotMessages = 0;

            foreach (var m in messageList)
            {
                Debug.WriteLine("m.Title: " + m.Title);
                foreach (Distribuerade_System_Labb_2User u in Us)
                {
                    var mUserID = GetUserById(m.SenderId);
                    Debug.WriteLine("m.SenderId: " + m.SenderId);
                    Debug.WriteLine("mUserID: " + mUserID);
                    if (mUserID.Id.Equals(u.Id) && !users.Contains(u))
                    {
                        users.Add(u);
                        TotMessages++;
                    }
                }
            }
            ViewBag.NoOfMessages = TotMessages;
            return View(users);
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
                bool result = await messageLogic.CreateNewMessage(message.Title, message.Body, message.ReceiverId, currentUser.Id);
               // TempData["ConfirmationMessage"] = "Meddelande nummer " + currentUser.Messages.Last().Id + " avsänt till " 
                //    + GetUserById(message.ReceiverId).UserName + ", " + DateTime.Now.ToString("H:mm yyyy-MM-dd");
                return RedirectToAction("Create");
            }
            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create2([Bind("Title,Body,ReceiverId")] Message message)
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
                TempData["ConfirmationMessage"] = "Meddelande nummer " + currentUser.Messages.Last().Id + " avsänt till "
                    + GetUserById(message.ReceiverId).UserName + ", " + DateTime.Now.ToString("H:mm yyyy-MM-dd");
                return RedirectToAction("Create");
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
            int messageId = id ?? default(int);

            bool result = await messageLogic.DeleteMessage(messageId);
           
            if (result == false)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));            
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
                var messages =  _context.Messages.ToList();
                int SumDeleted = 0;
                foreach (Message message in messages)
                {
                    if (message.Deleted != false) //Tar bort personen själv i listan
                        SumDeleted++;
                }
                ViewBag.NoOfDeletedMessages = SumDeleted;
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

        private async Task<List<MessageViewModel>> GetAllMessages()
        {
            List<MessageViewModel> messageList = new List<MessageViewModel>();
            var messages = await messageLogic.GetAllMessages();
            if (messages.Count > 0)
            {
                foreach (var m in messages)
                {
                    Debug.WriteLine("m2.Title: " + m.SenderId);
                    MessageViewModel currentMessage = new MessageViewModel
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Body = m.Body,
                        Read = m.Read,
                        Deleted = m.Deleted,
                        SentDate = m.SentDate,
                        ReceiverId = m.ReceiverId,
                        SenderId = m.SenderId
                    };
                    messageList.Add(currentMessage);
                }
            }
            return messageList;
        }


        private bool IsMessageForMe(MessageViewModel message)
        {
            string currentUserId = _userManager.GetUserId(User);
            return message.ReceiverId.Equals(currentUserId);
        }

        private bool IsItemByCurrentUser(MessageViewModel message)
        {
            string currentUserId = _userManager.GetUserId(User);
            return message.SenderId.Equals(currentUserId);
        }
    }
}
