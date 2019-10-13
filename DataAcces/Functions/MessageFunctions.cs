using DataAcces.DataContext;
using DataAcces.Entities;
using DataAcces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAcces.Functions
{
    public class MessageFunctions : IMessage
    {
        public async Task<Message> AddMessage(string title, string body, string receiverId, string senderId)
        {
            var context = new DatabaseContext(DatabaseContext.ops.dbOptions);
            //User currentUser = context.Users.Find(currentUserId);
            Message newMessage = new Message
            {
                Title = title,
                Body = body,
                ReceiverId = receiverId,
                Deleted = false,
                Read = false,
                SentDate = DateTime.Now,
                SenderId = senderId,
            };
            using (context)
            {
                await context.Messages.AddAsync(newMessage);
                await context.SaveChangesAsync();
            }
            return newMessage;
        }

        public async Task<List<Message>> GetAllMessages()
        {
            List<Message> messages = new List<Message>();
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                messages = await context.Messages.ToListAsync();
            }
            return messages;
        }
    }
}
