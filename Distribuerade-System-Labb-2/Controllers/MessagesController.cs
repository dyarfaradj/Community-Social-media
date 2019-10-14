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
            Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);

            if (id.Equals(null) || id.Equals(""))
            {
                return NotFound();
            }

            List<MessageViewModel> messages = await GetAllMessages(id);
            return View(messages);
        }
        public async Task<IActionResult> Index()
        {
            List<MessageViewModel> messageList = await GetAllMessages();
            Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
            List<Distribuerade_System_Labb_2User> users = new List<Distribuerade_System_Labb_2User>();
            int SumDeleted = await messageLogic.HowManyMessageDeleted(currentUser.Id);
            int SumRead = await messageLogic.HowManyMessagesRead(currentUser.Id);
            int TotMessages = await messageLogic.HowManyMessages(currentUser.Id);

            foreach (var m in messageList)
            {
                    var mUserID = GetUserById(m.SenderId);
                    if (m.ReceiverId.Equals(currentUser.Id))
                    {
                        var user = GetUserById(m.SenderId);
                        if(!users.Contains(user))
                        {
                            users.Add(user);
                        }
                    }
            }
            ViewBag.NoOfDeletedMessages = SumDeleted;
            ViewBag.NoOfMessages = TotMessages;
            ViewBag.NoOfReadMessages = SumRead;
            return View(users);
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int messageId = id ?? default(int);

            var m = await messageLogic.ReadMessage(messageId);
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

            return View(currentMessage);

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
        public async Task<IActionResult> Create([Bind("Title,Body,ReceiverId")] MessageViewModel message)
        {
            Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                int result = await messageLogic.CreateNewMessage(message.Title, message.Body, message.ReceiverId, currentUser.Id);
                TempData["ConfirmationMessage"] = "Meddelande nummer " + result + " avsänt till " 
                   + GetUserById(message.ReceiverId).UserName + ", " + DateTime.Now.ToString("HH:mm yyyy-MM-dd");
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
                    Debug.WriteLine("m2.Title: " + m.Title);
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
        private async Task<List<MessageViewModel>> GetAllMessages(String Id)
        {
            List<MessageViewModel> messageList = new List<MessageViewModel>();
            var messages = await messageLogic.GetAllMessages(Id);
            if (messages.Count > 0)
            {
                foreach (var m in messages)
                {
                    Debug.WriteLine("m2.Title: " + m.Title);
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
