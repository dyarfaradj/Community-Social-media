using DataAcces.DataContext;
using DataAcces.Entities;
using DataAcces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                MemberIds = null
            };
            using (context)
            {
                await context.Groups.AddAsync(newGroup);
                await context.SaveChangesAsync();
            }
            return newGroup;
        }

        public async Task<Boolean> RegisterUserToGroup(int groupID, string userId)
        {
            var context1 = new DatabaseContext(DatabaseContext.ops.dbOptions);
            Group currentGroup = await context1.Groups.FirstOrDefaultAsync(g => g.Id == groupID);
            Debug.WriteLine("GROUP ID:     " + groupID +" USERid: "+userId);


            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                // create the *NEW* Student
                var newGroupMember = new GroupMember
                {
                    MemberId = userId
                };
                currentGroup.MemberIds.Add(newGroupMember);
                context.GroupMembers.Add(newGroupMember);
                context.SaveChanges();
            }
            return true;

            //context.Groups.Add(newGroupMember);
            //currentGroup.MemberIds.Add();
            //try
            //{
            //    context.Update(currentGroup);
            //    await context.SaveChangesAsync();
            //    return true;
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    Debug.WriteLine("Group not found");
            //    return false;
            //}
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
