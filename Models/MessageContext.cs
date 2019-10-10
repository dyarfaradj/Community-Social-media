using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distribuerade_System_Labb_2.Models
{
    public class MessageContext : DbContext
    {
        public MessageContext(DbContextOptions<MessageContext> options):base(options)
        {

        }

        public DbSet<Message> Messages { get; set; }
    }
}
