using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Models
{
    public partial class UserType
    {
        public UserType()
        {
            Users = new HashSet<User>();
        }

        public int UserTypeId { get; set; }
        public string Typeuser { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
