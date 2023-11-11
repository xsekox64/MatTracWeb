using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.Models.Customers
{
    public class CustomerResponceDTO
    {
        public List<CustomerReadDto> CustomerResponceDtoList { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}