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
        /// <summary>
        /// GET INDEX
        /// </summary>
        /// <returns>Conversations</returns>
        public async Task<IActionResult> Index()
        {
            Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
            List<GroupViewModel> groups = await GetAllGroups(currentUser.Id);
            return View(groups);
        }

        /// <summary>
        /// GET All groups of user logged in 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> MyGroups()
        {
            Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
            List<GroupViewModel> groups = await GetAllMyOwnedGroups(currentUser.Id);
            return View(groups);
        }

        /// <summary>
        /// GET List of joined groups
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> JoinedGroup()
        {
            Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
            List<GroupViewModel> groups = await GetAllJoinedGroup(currentUser.Id);
            return View(groups);
        }


       /// <summary>
       /// GET Create group page
       /// </summary>
       /// <returns></returns>
        public IActionResult CreateGroup()
        {
            return View();
        }

        /// <summary>
        /// POST new group
        /// </summary>
        /// <param name="group">group model</param>
        /// <returns></returns>
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

        /// <summary>
        /// GET SendMessagePage details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// POST Sends messages to multiple users
        /// </summary>
        /// <param name="sendMessageViewModel">sendmessage model</param>
        /// <returns></returns>
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
                    result = await messageLogic.CreateNewMessage(sendMessageViewModel.TitleMessage, sendMessageViewModel.Body, sendMessageViewModel.ReceiverId, currentUser.Id);
                }
                TempData["ConfirmationMessage"] = "Grupp meddelande  avsänt till "
                   + String.Join(", ", usersSentTo.ToArray()) + ", " + DateTime.Now.ToString("HH:mm yyyy-MM-dd");
                return RedirectToAction("CreateGroup");
            }
            return RedirectToAction("CreateGroup");
        }

        /// <summary>
        /// GET Join group page
        /// </summary>
        /// <param name="id">Id of group</param>
        /// <returns></returns>
        public async Task<IActionResult> JoinGroup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int groupId = id ?? default(int);
            Distribuerade_System_Labb_2User currentUser = await _userManager.GetUserAsync(User);
            bool result = await groupLogic.RegisterToGroup(groupId, currentUser.Id);
            return RedirectToAction("Index");

        }

        /// <summary>
        /// Members To User Mapping
        /// </summary>
        /// <param name="memberList">model</param>
        /// <returns></returns>
        private async Task<List<Distribuerade_System_Labb_2User>> MemberToUserList(List<GroupMemberViewModel> memberList)
        {
            List<Distribuerade_System_Labb_2User> userList = new List<Distribuerade_System_Labb_2User>();
            foreach (var m in memberList)
            {
                
                userList.Add(GetUserById(m.UserId));
            }
            return userList;
        }
        /// <summary>
        /// Gets User by id
        /// </summary>
        /// <param name="id">id of user</param>
        /// <returns></returns>
        private Distribuerade_System_Labb_2User GetUserById(String id)
        {
            return _context.Users.Find(id);
        }

        /// <summary>
        /// Gets all members of a group
        /// </summary>
        /// <param name="groupId">group id</param>
        /// <returns></returns>
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
                            if(g.Id == gMember.GroupId)
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

        private async Task<List<GroupViewModel>> GetAllJoinedGroup(string userId)
        {
            List<GroupViewModel> groupList = new List<GroupViewModel>();
            List<GroupMemberViewModel> groupMembers = new List<GroupMemberViewModel>();
            var groups = await groupLogic.GetAllMyJoinedGroup(userId);
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
