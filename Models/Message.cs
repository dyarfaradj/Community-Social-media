using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Disribuerade_System_Labb_2.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Body { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public Boolean Read { get; set; }
        public Boolean Deleted { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public DateTime Sent { get; set; }

        [ForeignKey("AppUser")]
        public string SenderId { get; set; }

        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }
    }
}
