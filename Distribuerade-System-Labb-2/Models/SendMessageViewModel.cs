using Distribuerade_System_Labb_2.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Distribuerade_System_Labb_2.Models
{
    public class SendMessageViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        [Required(ErrorMessage = "Enter title for the message")]
        public string TitleMessage { get; set; }
        [Required(ErrorMessage = "Type something in the mail")]
        public string ReceiverId { get; set; }
        [Required(ErrorMessage = "Type something in the mail")]
        public string Body { get; set; }
        [Required(ErrorMessage = "Select one or more people")]
        public string[] SelectedValues { get; set; }
        public IEnumerable<SelectListItem> Values { get; set; }
    }
}
