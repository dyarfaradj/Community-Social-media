using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataAcces.Interfaces;
using DataAcces.Entities;
using System.Diagnostics;

namespace BusinessLogic.MessageLogic
{
    public class MessageLogic
    {
        private readonly IMessage _message = new DataAcces.Functions.MessageFunctions();


        public async Task<int> CreateNewMessage(string title, string body, string receiverId, string currentUserId)
        {
            try
            {
                var result = await _message.AddMessage(title, body, receiverId, currentUserId);
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

        public async Task<Boolean> DeleteMessage(int messageId)
        {
            try
            {
                var result = await _message.DeleteMessage(messageId);
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
        public async Task<Message> ReadMessage(int messageId)
        {
            try
            {
                Message result = await _message.ReadMessage(messageId);
                
                    return result;
                
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return null;
            }
        }

        public async Task<Message> GetMessage(int messageId)
        {
            try
            {
                Message result = await _message.GetMessage(messageId);

                return result;

            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return null;
            }
        }

        public async Task<List<Message>> GetAllMessages()
        {
            List<Message> messages = await _message.GetAllMessages();
            List<Message> messagesList = new List<Message>();
            foreach (Message m in messages)
            {
                if (m.Deleted == false)
                    messagesList.Add(m);
            }

            return messagesList;
        }
        public async Task<List<Message>> GetAllMessagesFrom(String senderId, String receiverID)
        {
            List<Message> messages = await _message.GetAllMessages();
            List<Message> messagesList = new List<Message>();
            foreach (Message m in messages)
            {
                if (m.Deleted == false && m.SenderId == senderId && m.ReceiverId == receiverID)
                    messagesList.Add(m);
            }

            return messagesList;
        }

        public async Task<List<Message>> GetAllMessagesTo(String Id)
        {
            List<Message> messages = await _message.GetAllMessages();
            List<Message> messagesList = new List<Message>();
            foreach (Message m in messages)
            {
                if (m.Deleted == false && m.ReceiverId == Id)
                    messagesList.Add(m);
            }

            return messagesList;
        }

        public async Task<int> HowManyMessages(String Id)
        {
            List<Message> messages = await _message.GetAllMessages();
            int total = 0;
            foreach (Message m in messages)
            {
                if (m.ReceiverId.Equals(Id))
                    if (m.Deleted == false)
                    total++;
            }
            return total;
        }
        public async Task<int> HowManyMessageDeleted(String Id)
        {
            List<Message> messages = await _message.GetAllMessages();
            int total = 0;
            foreach (Message m in messages)
            {
                if (m.ReceiverId.Equals(Id))
                    if (m.Deleted != false)
                    total++;
            }
            return total;
        }
        public async Task<int> HowManyMessagesRead(String Id)
        {
            List<Message> messages = await _message.GetAllMessages();
            int total = 0;
            foreach (Message m in messages)
            {
                if (m.ReceiverId.Equals(Id))
                    if (m.Deleted == false && m.Read != false)
                        total++;
            }
            return total;
        }
        public async Task<int> HowManyMessagesUnRead(String Id)
        {
            List<Message> messages = await _message.GetAllMessages();
            int total = 0;
            foreach (Message m in messages)
            {
                if (m.ReceiverId.Equals(Id))
                    if (m.Deleted == false && m.Read == false)
                        total++;
            }
            return total;
        }
    }
}
 