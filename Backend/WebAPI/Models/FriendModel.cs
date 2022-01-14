using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class FriendModel
    {
        public int FriendId { get; set; }
        public int UserId { get; set; }
        public bool status { get; set; }
        public string FriendKey { get; set; }
        public string ImgURL { get; set; }
        public string Name { get; set; }
        public string DateSend { get; set; }
        public int CountUnRead { get; set; }
        public List<int> IdMessageNew { get; set; }
        public string MessageNew { get; set; }
        public DateTime sortDate { get; set; }
        public string ColorSeen { get; set; }
        public bool IsSeen { get; set; }
        public bool IsChecked { get; set; }
        public bool AcceptFriend { get; set; }
    }
}
