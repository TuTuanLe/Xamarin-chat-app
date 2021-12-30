using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Models
{
    public partial class UserGroup
    {
        public UserGroup()
        {
            DetailUserGroups = new HashSet<DetailUserGroup>();
        }

        public int UserGroupId { get; set; }
        public int? UserId { get; set; }
        public byte? IsAdmin { get; set; }
        public string NickNameowner { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<DetailUserGroup> DetailUserGroups { get; set; }
    }
}
