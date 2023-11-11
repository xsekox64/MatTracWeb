using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.Models.Customers
{
    public class CustomerReadDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime AddDateTime { get; set; } = DateTime.Now;
        public string CustomerTcNo { get; set; }
        public bool Active { get; set; }
        public string VergiNo { get; set; }
        public string VergiDairesi { get; set; }
    }
}