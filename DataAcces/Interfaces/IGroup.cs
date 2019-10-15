using DataAcces.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAcces.Interfaces
{
    public interface IGroup
    {
        Task<Boolean> RegisterUserToGroup(int groupID, string userId);
        Task<Group> AddGroup(string groupTitle, string ownerId);

        Task<Group> GetGroup(int groupId);
        Task<List<Member>> GetAllMembers();
        Task<List<Group>> GetAllGroups();
    }
}
