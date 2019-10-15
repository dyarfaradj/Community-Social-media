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
        }
        public async Task<List<Member>> GetAllMembers()
        {
            List<Member> members = new List<Member>();
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                members = await context.Members.ToListAsync();
            }
            return members;
        }

        public async Task<Group> GetGroup(int groupId)
        {
            var context = new DatabaseContext(DatabaseContext.ops.dbOptions);
            Group group = context.Groups.FirstOrDefault(contextGroup => contextGroup.Id == groupId);
            return group;
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
