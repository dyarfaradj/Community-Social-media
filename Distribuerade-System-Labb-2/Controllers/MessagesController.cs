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
using Microsoft.AspNetCore.Http;

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

            List<MessageViewModel> messages = await GetAllMessagesFrom(id, currentUser.Id);
            return View(messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MessageOfUser(List<MessageViewModel> messages)
        {
            if (ModelState.IsValid)
            {
                foreach (MessageViewModel m in messages)
                {
                    if (m.Read)
                    {
                        await messageLogic.ReadMessage(m.Id);
                    }
                }
                return RedirectToAction("MessageOfUser");
            }
            return RedirectToAction("MessageOfUser");
        }

        public async Task<IActionResult> Index()
        {
            Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
            List<MessageViewModel> messageList = await GetAllMessagesTo(currentUser.Id);
            List<Distribuerade_System_Labb_2User> users = new List<Distribuerade_System_Labb_2User>();
            int SumDeleted = await messageLogic.HowManyMessageDeleted(currentUser.Id);
            int SumRead = await messageLogic.HowManyMessagesRead(currentUser.Id);
            int SumUnRead = await messageLogic.HowManyMessagesUnRead(currentUser.Id);
            int TotMessages = await messageLogic.HowManyMessages(currentUser.Id);

            foreach (var m in messageList)
            {
                var user = GetUserById(m.SenderId);
                if(!users.Contains(user))
                {
                    users.Add(user);
                }
            }
            ViewBag.NoOfDeletedMessages = SumDeleted;
            ViewBag.NoOfMessages = TotMessages;
            ViewBag.NoOfReadMessages = SumRead;
            ViewBag.NoOfUnReadMessages = SumUnRead;
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
            bool isItForMe = await IsMessageForMe(messageId);
            if (!isItForMe)
            {
                return NotFound();
            }
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
            var usernameList = new SendMessageViewModel();
            string currentUserId = _userManager.GetUserId(User);

            usernameList.Values = users.Where(u => u.Id != currentUserId).Select((u, index) =>  new SelectListItem { Value = u.Id, Text = u.UserName });

            return View(usernameList);
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("TitleMessage,Body,SelectedValues")] SendMessageViewModel sendMessageViewModel)
        {
             Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
            if(ModelState.IsValid)
            {
                List<string> usersSentTo = new List<string>();
                int result = 0;
                foreach (var u in sendMessageViewModel.SelectedValues)
                {
                    sendMessageViewModel.ReceiverId = u;
                    usersSentTo.Add(GetUserById(sendMessageViewModel.ReceiverId).UserName);
                    result = await messageLogic.CreateNewMessage(sendMessageViewModel.TitleMessage, sendMessageViewModel.Body, sendMessageViewModel.ReceiverId, currentUser.Id);
                }
                TempData["ConfirmationMessage"] = "Meddelande nummer " + result + " avsänt till " 
                   + String.Join(", ", usersSentTo.ToArray()) + ", " + DateTime.Now.ToString("HH:mm yyyy-MM-dd");
                return RedirectToAction("Create");
            }
            return RedirectToAction("Create");
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int messageId = id ?? default(int);
            bool isItForMe = await IsMessageForMe(messageId);
            if (!isItForMe)
            {
                return NotFound();
            }
            bool result = await messageLogic.DeleteMessage(messageId);

            if (result == false)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
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
        private async Task<List<MessageViewModel>> GetAllMessagesFrom(String senderId, String receiverID)
        {
            List<MessageViewModel> messageList = new List<MessageViewModel>();
            var messages = await messageLogic.GetAllMessagesFrom(senderId, receiverID);
            if (messages.Count > 0)
            {
                foreach (var m in messages)
                {
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

        private async Task<List<MessageViewModel>> GetAllMessagesTo(String Id)
        {
            List<MessageViewModel> messageList = new List<MessageViewModel>();
            var messages = await messageLogic.GetAllMessagesTo(Id);
            if (messages.Count > 0)
            {
                foreach (var m in messages)
                {
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


        private async Task<bool> IsMessageForMe(int messageId)
        {
            var message = await messageLogic.GetMessage(messageId);
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
