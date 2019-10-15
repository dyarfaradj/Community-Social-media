using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAcces.Entities
{
    public class GroupMember
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string MemberId { get; set; }
    }
}
