using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Distribuerade_System_Labb_2.Models
{
    [NotMapped]
    public class UserViewModel : IdentityUser
    {
        public class ApplicatioUserViewModelnUser : IdentityUser
        {
            public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<UserViewModel> manager, string authenticationType)
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
    }
}
