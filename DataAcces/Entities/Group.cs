using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAcces.Entities
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string GroupTitle { get; set; }
        [Required]
        public string OwnerId { get; set; }
        public virtual List<Member> Members { get; } = new List<Member>();
    }
}
