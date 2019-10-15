using Distribuerade_System_Labb_2.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Distribuerade_System_Labb_2.Models
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please enter a group title")]
        [StringLength(60, MinimumLength = 3)]
        public string GroupTitle { get; set; }
        public string OwnerId { get; set; }
        public virtual List<GroupMemberViewModel> MemberIds { get; set; }
    }
}
