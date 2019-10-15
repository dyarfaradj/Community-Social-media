using Distribuerade_System_Labb_2.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distribuerade_System_Labb_2.Models
{
    public class SendMessageViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string[] SelectedValues { get; set; }
        public IEnumerable<SelectListItem> Values { get; set; }
    }
}
