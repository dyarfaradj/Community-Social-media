using Microsoft.AspNetCore.Identity.EntityFrameworkCore;using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace DataAcces.Entities
{
    public class User : IdentityUser
    {
        public int LoginPerMonth { get; set; }
        public DateTimeOffset LastLoginDate { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
