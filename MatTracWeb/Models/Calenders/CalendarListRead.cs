using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.Models.Calenders
{
    public class CalendarListRead
    {
        public int CalendarId { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string endFin { get; set; }
        public string allDay { get; set; }
        public string url { get; set; }
        public string decsr { get; set; }
        public string hour { get; set; }
        public int CompanyId { get; set; }
        public int InterviewType { get; set; }
    }
}