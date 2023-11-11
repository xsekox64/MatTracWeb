using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.Models
{
    public class EmailControlUpdate
    {       
        public int Count { get; set; }
        public int UserId { get; set; }
        public DateTime SendDate { get; set; } = DateTime.Now;
        public int Code { get; set; }
    }
}