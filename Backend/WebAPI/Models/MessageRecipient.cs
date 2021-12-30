using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Models
{
    public partial class MessageRecipient
    {
        public int RecipientId { get; set; }
        public int? MessageId { get; set; }
        public int? UserId { get; set; }

        public virtual Messaging Message { get; set; }
        public virtual User User { get; set; }
    }
}
