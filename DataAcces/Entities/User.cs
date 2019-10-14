using Microsoft.AspNetCore.Identity.EntityFrameworkCore;using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcces.Entities
{
    [NotMapped]
    public class User : IdentityUser
    {
        public class ApplicationUser : IdentityUser
        {
            public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
            {
                // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
                //var iudentiy = await manager.CreateIdentityAsync(this, authenticationType);
                var userIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                // Add custom user claims here
                return userIdentity;
            }
        }


        public int LoginPerMonth { get; set; }
        public DateTimeOffset LastLoginDate { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
