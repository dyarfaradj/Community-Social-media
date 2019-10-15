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
using System.Diagnostics;

namespace Distribuerade_System_Labb_2.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private readonly GroupLogic groupLogic = new GroupLogic();
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


        private async Task<List<GroupViewModel>> GetAllGroups(string userId)
        {
            List<GroupViewModel> groupList = new List<GroupViewModel>();
            List<GroupMemberViewModel> groupMembers = new List<GroupMemberViewModel>();
            var groups = await groupLogic.GetAllGroups(userId);
            if (groups.Count > 0)
            {
                foreach (var g in groups)
                {
                    if(g.MemberIds != null)
                    {
                        foreach (var gMember in g.MemberIds)
                        {
                            GroupMemberViewModel currentMember = new GroupMemberViewModel
                            {
                                Id = gMember.Id,
                                MemberId = gMember.MemberId
                            };
                            groupMembers.Add(currentMember);
                        }
                        GroupViewModel currentGroup = new GroupViewModel
                        {
                            Id = g.Id,
                            GroupTitle = g.GroupTitle,
                            OwnerId = g.OwnerId,
                            // MemberIds = groupList.Select((u, index) => new GroupMemberViewModel {Id = u.Id, MemberId = u.MemberId});
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
