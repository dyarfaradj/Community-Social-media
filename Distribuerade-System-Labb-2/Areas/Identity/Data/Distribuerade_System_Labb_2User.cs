using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Distribuerade_System_Labb_2.Models;
using Microsoft.AspNetCore.Identity;

namespace Distribuerade_System_Labb_2.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the Distribuerade_System_Labb_2User class
    public class Distribuerade_System_Labb_2User : IdentityUser
    {
        public int LoginPerMonth { get; set; }
        public DateTimeOffset LastLoginDate { get; set; }
    }
}
