using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataAcces.Interfaces;
using DataAcces.Entities;

namespace BusinessLogic.UserLogic
{
    public class UserLogic
    {
        private readonly IUser _user = new DataAcces.Functions.UserFunctions();

        public async Task<User> GetUserById(string id)
        {
            User user =  _user.GetUserById(id);
            return user;
        }

        public async Task<List<User>> GetAllUsers()
        {
            List<User> users = await _user.GetAllUsers();
            return users;
        }
    }
}
