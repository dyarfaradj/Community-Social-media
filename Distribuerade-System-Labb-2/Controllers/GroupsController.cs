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
using BusinessLogic.GroupLogic;
using BusinessLogic.MessageLogic;
using System.Diagnostics;

namespace Distribuerade_System_Labb_2.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private readonly GroupLogic groupLogic = new GroupLogic();
        private readonly MessageLogic messageLogic = new MessageLogic();
        private readonly Distribuerade_System_Labb_2Context _context;
        private readonly UserManager<Distribuerade_System_Labb_2User> _userManager;

        public GroupsController(Distribuerade_System_Labb_2Context context, UserManager<Distribuerade_System_Labb_2User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
            List<GroupViewModel> groups = await GetAllGroups(currentUser.Id);
            return View(groups);
        }

        public async Task<IActionResult> MyGroups()
        {
            Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
            List<GroupViewModel> groups = await GetAllMyOwnedGroups(currentUser.Id);
            return View(groups);
        }


        // GET: Messages/Create
        public IActionResult CreateGroup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateGroup([Bind("GroupTitle")] GroupViewModel group)
        {
            Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                int result = await groupLogic.CreateNewGroup(group.GroupTitle, currentUser.Id);
                TempData["ConfirmationMessage"] = "Group: " + group.GroupTitle + " was created!";
                return RedirectToAction("CreateGroup");
            }
            return RedirectToAction("CreateGroup");
        }

        // GET: Messages/Create
        public async Task<IActionResult> SendGroupMessage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int groupId = id ?? default(int);

            List <GroupMemberViewModel> listOfGroupMembers = await GetAllMembersOfGroup(groupId);
            List<Distribuerade_System_Labb_2User> userList = await MemberToUserList(listOfGroupMembers);
            var usernameList = new SendMessageViewModel();
            usernameList.Values = userList.Select((u, index) => new SelectListItem { Value = u.Id, Text = u.UserName, Selected = true });
            return View(usernameList);
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendGroupMessage([Bind("TitleMessage,Body,SelectedValues")] SendMessageViewModel sendMessageViewModel)
        {
            Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                List<string> usersSentTo = new List<string>();
                int result = 0;
                foreach (var u in sendMessageViewModel.SelectedValues)
                {
                    sendMessageViewModel.ReceiverId = u;
                    usersSentTo.Add(GetUserById(sendMessageViewModel.ReceiverId).UserName);
                    Debug.WriteLine("Message: " + sendMessageViewModel.TitleMessage + " " + sendMessageViewModel.Body + " " + sendMessageViewModel.ReceiverId + " " + currentUser.Id);
                    result = await messageLogic.CreateNewMessage(sendMessageViewModel.TitleMessage, sendMessageViewModel.Body, sendMessageViewModel.ReceiverId, currentUser.Id);
                }
                TempData["ConfirmationMessage"] = "Grupp meddelande  avsänt till "
                   + String.Join(", ", usersSentTo.ToArray()) + ", " + DateTime.Now.ToString("HH:mm yyyy-MM-dd");
                return RedirectToAction("CreateGroup");
            }
            return RedirectToAction("CreateGroup");
        }

        public async Task<IActionResult> JoinGroup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int groupId = id ?? default(int);
            Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
            Debug.WriteLine("zGROUP ID:     " + groupId + " USERid: " + currentUser.Id);
            bool result = await groupLogic.RegisterToGroup(groupId, currentUser.Id);
            return RedirectToAction("Index");

        }
        private async Task<List<Distribuerade_System_Labb_2User>> MemberToUserList(List<GroupMemberViewModel> memberList)
        {
            List<Distribuerade_System_Labb_2User> userList = new List<Distribuerade_System_Labb_2User>();
            foreach (var m in memberList)
            {
                
                userList.Add(GetUserById(m.UserId));
            }
            return userList;
        }
        private Distribuerade_System_Labb_2User GetUserById(String id)
        {
            return _context.Users.Find(id);
        }

        private async Task<List<GroupMemberViewModel>> GetAllMembersOfGroup(int groupId)
        {
            List<GroupMemberViewModel> groupMemberList = new List<GroupMemberViewModel>();
            var members = await groupLogic.GetAllMembersOfGroup(groupId);
            if (members.Count > 0)
            {
                foreach (var m in members)
                {
                    GroupMemberViewModel currentMember = new GroupMemberViewModel
                    {
                        Id = m.Id,
                        UserId = m.UserId

                    };
                    groupMemberList.Add(currentMember);
                }
            }
            return groupMemberList;
        }


        private async Task<List<GroupViewModel>> GetAllGroups(string userId)
        {
            List<GroupViewModel> groupList = new List<GroupViewModel>();
            List<GroupMemberViewModel> groupMembers = new List<GroupMemberViewModel>();
            var groups = await groupLogic.GetAllGroups(userId);
            if (groups.Count > 0)
            {
                foreach (var g in groups)
                {
                    if(g.Members != null)
                    {
                        foreach (var gMember in g.Members)
                        {
                            GroupMemberViewModel currentMember = new GroupMemberViewModel
                            {
                                Id = gMember.Id,
                                UserId = gMember.UserId
                            };
                            groupMembers.Add(currentMember);
                        }
                        GroupViewModel currentGroup = new GroupViewModel
                        {
                            Id = g.Id,
                            GroupTitle = g.GroupTitle,
                            OwnerId = g.OwnerId,
                            MemberIds = groupMembers

                        };
                        groupList.Add(currentGroup);
                    }
                    else
                    {
                        GroupViewModel currentGroup = new GroupViewModel
                        {
                            Id = g.Id,
                            GroupTitle = g.GroupTitle,
                            OwnerId = g.OwnerId,
                        };
                        groupList.Add(currentGroup);
                    }
                }
            }
            return groupList;
        }

        private async Task<List<GroupViewModel>> GetAllMyOwnedGroups(string userId)
        {
            List<GroupViewModel> groupList = new List<GroupViewModel>();
            List<GroupMemberViewModel> groupMembers = new List<GroupMemberViewModel>();
            var groups = await groupLogic.GetAllMyOwnedGroups(userId);
            if (groups.Count > 0)
            {
                foreach (var g in groups)
                {
                    if (g.Members != null)
                    {
                        foreach (var gMember in g.Members)
                        {
                            GroupMemberViewModel currentMember = new GroupMemberViewModel
                            {
                                Id = gMember.Id,
                                UserId = gMember.UserId
                            };
                            groupMembers.Add(currentMember);
                        }
                        GroupViewModel currentGroup = new GroupViewModel
                        {
                            Id = g.Id,
                            GroupTitle = g.GroupTitle,
                            OwnerId = g.OwnerId,
                            MemberIds = groupMembers

                        };
                        groupList.Add(currentGroup);
                    }
                    else
                    {
                        GroupViewModel currentGroup = new GroupViewModel
                        {
                            Id = g.Id,
                            GroupTitle = g.GroupTitle,
                            OwnerId = g.OwnerId,
                        };
                        groupList.Add(currentGroup);
                    }
                }
            }
            return groupList;
        }
    }


}
