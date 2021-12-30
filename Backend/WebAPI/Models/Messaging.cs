using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Models
{
    public partial class Messaging
    {
        public Messaging()
        {
            MessageRecipients = new HashSet<MessageRecipient>();
        }

        public int MessageId { get; set; }
        public int? FromUserId { get; set; }
        public DateTime DateSent { get; set; }
        public DateTime? DateRead { get; set; }
        public string Content { get; set; }
        public string AttachedFiles { get; set; }
        public int? ToUserId { get; set; }
        public string FriendId { get; set; }
        public virtual User FromUser { get; set; }
        public virtual ICollection<MessageRecipient> MessageRecipients { get; set; }
    }
}
