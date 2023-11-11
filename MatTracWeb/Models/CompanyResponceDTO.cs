using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.Models
{
    public class CompanyResponceDTO
    {
        public List<CompanyReadDto> CompanyResponceDtoList { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}