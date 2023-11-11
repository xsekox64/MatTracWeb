using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.ViewModels.ServiceInfo
{
    public class ServicesParcaFiyatListResponceDto
    {
        public List<ServicesParcaFiyatListReadDto> ServicesParcaFiyatListReadDto { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}