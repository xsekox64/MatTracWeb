using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.ViewModels.ServiceInfo
{
    public class ServiceHistoryListResponceDto
    {
        public List<ServiceHistoryListReadDto> ServiceHistoryListReadDtoList { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}