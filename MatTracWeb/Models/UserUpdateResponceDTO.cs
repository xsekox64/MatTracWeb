using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.Models
{
    public class UserUpdateResponceDTO
    {
        public UsersUpdateDto userUpdateResponceDtoList { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}