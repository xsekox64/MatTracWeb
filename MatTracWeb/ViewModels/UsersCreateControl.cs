using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.ViewModels
{
    public class UsersCreateControl
    {
        public string UserName { get; set; }
        public string UserSurname { get; set; }       
        public string Email { get; set; }
        public string UserAuthorization { get; set; }        
        public string TcNo { get; set; }
        public string Phone { get; set; } 
        public string WebPlatform { get; set; }
        public string AndoridPlatform { get; set; }
        public string IOSPlatform { get; set; }
    }
}