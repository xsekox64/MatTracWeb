using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.ViewModels
{
    public class CustomerCarsInfoResponceDTO
    {
        public List<CustomerCarsInfoReadDto> CustomerResponceDtoList { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}