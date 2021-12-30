using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Models
{
    public partial class User
    {
        public User()
        {
            DetailUserGroups = new HashSet<DetailUserGroup>();
            FriendUserIdfriendNavigations = new HashSet<Friend>();
            FriendUsers = new HashSet<Friend>();
            MessageRecipients = new HashSet<MessageRecipient>();
            Messagings = new HashSet<Messaging>();
            UserGroups = new HashSet<UserGroup>();
        }

        public int UserId { get; set; }
        public int? UserIdtype { get; set; }
        public byte? Active { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Passwordd { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string ImgURL { get; set; }

        public virtual UserType UserIdtypeNavigation { get; set; }
        public virtual ICollection<DetailUserGroup> DetailUserGroups { get; set; }
        public virtual ICollection<Friend> FriendUserIdfriendNavigations { get; set; }
        public virtual ICollection<Friend> FriendUsers { get; set; }
        public virtual ICollection<MessageRecipient> MessageRecipients { get; set; }
        public virtual ICollection<Messaging> Messagings { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}
