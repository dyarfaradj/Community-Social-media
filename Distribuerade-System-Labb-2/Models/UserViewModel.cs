using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distribuerade_System_Labb_2.Models
{
    public class UserViewModel
    {
        public int LoginPerMonth { get; set; }
        public DateTimeOffset LastLoginDate { get; set; }
    }
}
