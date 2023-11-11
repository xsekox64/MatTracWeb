using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.ViewModels
{
    public class CustomerCreateDto
    {
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerTcNo { get; set; }
        public string VergiNo { get; set; }
        public string VergiDairesi { get; set; }
    }
}