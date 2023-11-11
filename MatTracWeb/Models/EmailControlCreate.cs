using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.Models
{
    public class EmailControlCreate
    {
        public string Email { get; set; }
        public string Code { get; set; }       
        public int Count { get; set; }
        public int UserId { get; set; }
    }
}