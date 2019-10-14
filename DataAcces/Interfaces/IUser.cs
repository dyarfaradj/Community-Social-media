using DataAcces.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAcces.Interfaces
{
    public interface IUser
    {
        User GetUserById(string id);
        Task<List<User>> GetAllUsers();
    }
}
