using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.Models.Calenders
{
    public class CalendarListUpdate
    {
        public int CalendarId { get; set; }
        public string title { get; set; }          
        public string decsr { get; set; }
        public string hour { get; set; }       
        public int InterviewType { get; set; }
    }
}