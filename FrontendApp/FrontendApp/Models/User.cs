using System;
using System.Collections.Generic;
using System.Text;

namespace FrontendApp.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Passwordd { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public string ImgURL { get; set; }
        public string Gmail { get; set; }
    }
}
