using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.Models
{
    public class EmailControlRead
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime SendDate { get; set; }
        public int Count { get; set; }
        public int UserId { get; set; }
    }
}