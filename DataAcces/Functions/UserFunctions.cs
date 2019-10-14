using DataAcces.DataContext;
using DataAcces.Entities;
using DataAcces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcces.Functions
{
    public class UserFunctions : IUser
    {
        public User GetUserById(string id)
        {
            var context = new DatabaseContext(DatabaseContext.ops.dbOptions);
            User user = context.Users.Find(id);
            return user;
        }

        public async Task<List<User>> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                users = await context.Users.ToListAsync();
            }
            return users;
        }
    }
}
