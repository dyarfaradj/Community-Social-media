using Distribuerade_System_Labb_2.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Distribuerade_System_Labb_2.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public Boolean Read { get; set; }
        public Boolean Deleted { get; set; }
        [Required]
        public DateTime SentDate { get; set; }

        [ForeignKey("Distribuerade_System_Labb_2User")]
        public string SenderId { get; set; }

        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }

        public virtual Distribuerade_System_Labb_2User User { get; set; }
    }
}
