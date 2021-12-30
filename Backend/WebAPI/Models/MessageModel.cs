using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class MessageModel
    {
        public int messsageId { get; set; }
        public int fromUserId { get; set; }
        public int toUserId { get; set; }
        public string NameUser { get; set; }
        public string ToNameUser { get; set; }
        public string Message { get; set; }
        public bool IsOwnMessage { get; set; }
        public bool IsSystemMessage { get; set; }
        public DateTime DateSent { get; set; }
        public DateTime? DateRead { get; set; }
        public string Content { get; set; }
        public string AttachedFiles { get; set; }
        public string ImgURLFromUser { get; set; }
        public string ImgURLToUser { get; set; }
    }
}
