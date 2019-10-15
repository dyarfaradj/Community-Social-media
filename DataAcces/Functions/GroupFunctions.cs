using DataAcces.DataContext;
using DataAcces.Entities;
using DataAcces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcces.Functions
{
    public class GroupFunctions : IGroup
    {
        public async Task<Group> AddGroup(string groupTitle, string ownerId)
        {
            var context = new DatabaseContext(DatabaseContext.ops.dbOptions);
            //User currentUser = context.Users.Find(currentUserId);
            Group newGroup = new Group
            {
                GroupTitle = groupTitle,
                OwnerId = ownerId,
            };
            using (context)
            {
                await context.Groups.AddAsync(newGroup);
                await context.SaveChangesAsync();
            }
            return newGroup;
        }

        public async Task<Boolean> RegisterUserToGroup(int groupId, string userId)
        {
            Debug.WriteLine("GROUP ID:  " + groupId + " USERid: " + userId);

            var context = new DatabaseContext(DatabaseContext.ops.dbOptions);

            Group group = context.Groups.FirstOrDefault(contextGroup => contextGroup.Id == groupId);
            if (group != null)
            {
                Member member = new Member
                {
                    Group = group,
                    UserId = userId,
                };
            
                context.Members.Add(member);
                context.SaveChanges();
                return true;
            }
            return false;


            ////Getting the group
            //Group currentGroup = await context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);

            ////Creating a groupmember and saving
            //if(CreateGroupMember(userId))
            //{
            //    //Add groupMember to group
            //    using (context)
            //    {
            //        //currentGroup.MemberIds.Add(context.GroupMembers.FirstOrDefault(g => g.MemberId == userId));
            //        context.SaveChanges();
            //    }
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        private Boolean CreateGroupMember(string userId)
        {
            try
            {
                using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
                {
                    var newGroupMember = new Member
                    {
                        UserId = userId
                    };
                    context.Members.Add(newGroupMember);
                    context.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<List<Group>> GetAllGroups()
        {
            List<Group> groups = new List<Group>();
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                groups = await context.Groups.ToListAsync();
            }
            return groups;
        }
    }
}
