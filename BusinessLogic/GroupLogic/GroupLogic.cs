using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataAcces.Interfaces;
using DataAcces.Entities;
using System.Diagnostics;

namespace BusinessLogic.GroupLogic
{
    public class GroupLogic
    {
        private readonly IGroup _group = new DataAcces.Functions.GroupFunctions();


        public async Task<int> CreateNewGroup(string groupTitle, string ownerId)
        {
            try
            {
                var result = await _group.AddGroup(groupTitle, ownerId);
                if(result.Id >0)
                { 
                    return result.Id;
                }
                else
                {
                    return 0;
                }
            }
            catch(Exception error)
            {
                Console.WriteLine(error);
                return 0;
            }
        }

        public async Task<Boolean> RegisterToGroup(int groupId, string userId)
        {
            try
            {
                var result = await _group.RegisterUserToGroup(groupId, userId);
                if (result)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }


        public async Task<List<Group>> GetAllGroups(string userId)
        {
            List<Group> groups = await _group.GetAllGroups();
            List<Group> groupsList = new List<Group>();
            foreach (Group g in groups)
            {
                if (g.OwnerId != userId)
                    groupsList.Add(g);
            }

            return groupsList;
        }
    }
}
 