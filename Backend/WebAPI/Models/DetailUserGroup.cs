using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Models
{
    public partial class DetailUserGroup
    {
        public int DetailId { get; set; }
        public int? UserGroupId { get; set; }
        public int? AddUserId { get; set; }
        public string NickNameGuest { get; set; }

        public virtual User AddUser { get; set; }
        public virtual UserGroup UserGroup { get; set; }
    }
}
