using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataAcces.Interfaces;
using DataAcces.Entities;

namespace BusinessLogic.MessageLogic
{
    public class MessageLogic
    {
        private readonly IMessage _message = new DataAcces.Functions.MessageFunctions();


        public async Task<Boolean> CreateNewMessage(string title, string body, string receiverId, string currentUserId)
        {
            try
            {
                var result = await _message.AddMessage(title, body, receiverId, currentUserId);
                if(result.Id >0)
                { 
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }

        public async Task<List<Message>> GetAllMessages()
        {
            List<Message> messages = await _message.GetAllMessages();
            return messages;
        }
    }
}
