using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.Models
{
    public class UsersCreateDto
    {
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public int CompanyId { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.Now;
        public string Email { get; set; }
        public int UserAuthorization { get; set; }
        public bool Active { get; set; } = true;
        public string TcNo { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int PasswordChange { get; set; } = 0;
        public bool WebPlatform { get; set; }
        public bool AndoridPlatform { get; set; }
        public bool IOSPlatform { get; set; }
    }
}