using System;
using System.Collections.Generic;
using System.Text;

namespace FrontendApp.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public int? UserIdtype { get; set; }
        public int? Active { get; set; }
        public string CompanyName { get; set; }
        public string FullName { get; set; }
        public string Passwordd { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string ImgURL { get; set; }
        public int? UserIdfriend { get; set; }
        public int? status { get; set; }
        public string FriendKey { get; set; }
        public string Gmail { get; set; }
    }
}
