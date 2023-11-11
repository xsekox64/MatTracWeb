using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.ViewModels.Services
{
    public class CustomerDemand
    {       
        
        public string tags_1 { get; set; }
        public int CustomerId { get; set; }
        public int CustomerCarsId { get; set; }
        public DateTime AddDateTime { get; set; } = DateTime.Now;
    }
}