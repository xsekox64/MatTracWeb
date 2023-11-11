using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.Models
{
    public class UsersUpdateDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string Email { get; set; }
        public int UserAuthorization { get; set; }
        public string TcNo { get; set; }
        public string Phone { get; set; }
        public bool WebPlatform { get; set; }
        public bool AndoridPlatform { get; set; }
        public bool IOSPlatform { get; set; }
    }
}