using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.Models
{
    public class UsersReadDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string TcNo { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int PasswordChange { get; set; }
        public string WebPlatform { get; set; }
        public string AndoridPlatform { get; set; }
        public string IOSPlatform { get; set; }
        public string AuthorizationTypeName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAdress { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyCounty { get; set; }
        public string LandPhone { get; set; }
        public string FaxNumber { get; set; }
        public string CellPhone { get; set; }
        public int UserAuthorization { get; set; }
        public int CompanyId { get; set; }
    }
}