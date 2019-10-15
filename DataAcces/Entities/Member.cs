using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAcces.Entities
{
    public class Member
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public Group Group{ get; set; }
        [Required]
        public int GroupId { get; set; }
    }
}
