using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Models
{
    public partial class Friend
    {
        public int FriendId { get; set; }
        public int? UserId { get; set; }
        public int? UserIdfriend { get; set; }
        public int? status { get; set; }
        public string FriendKey { get; set; }

        public virtual User User { get; set; }
        public virtual User UserIdfriendNavigation { get; set; }
    }
}
