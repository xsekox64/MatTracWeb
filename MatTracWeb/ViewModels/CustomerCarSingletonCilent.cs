using MatTracWeb.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.ViewModels
{
    public class CustomerCarSingletonCilent
    {
        public List<ServicesCustomerCars> CustomerCarSingletonDtoList { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}