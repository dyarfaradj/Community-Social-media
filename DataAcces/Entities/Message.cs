using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAcces.Entities
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
        public string ReceiverId { get; set; }
        public string SenderId { get; set; }
        public virtual User User { get; set; }
    }
}
