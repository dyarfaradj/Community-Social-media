using DataAcces.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAcces.Interfaces
{
    public interface IMessage
    {
        Task<Message> AddMessage(string title, string body, string receiverId, string currentUserId);
        Task<List<Message>> GetAllMessages();
    }
}
