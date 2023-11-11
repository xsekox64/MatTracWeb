using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.ViewModels.ServiceInfo
{
    public class ServiceHistoryListReadDto
    {
        public int Id { get; set; }
        public int CustomerCarsId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string Plaka { get; set; }       
        public string tags_1 { get; set; }
        public DateTime AddDateTime { get; set; }
    }
}