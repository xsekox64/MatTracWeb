﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.ViewModels.Services
{
    public class ServicesInvoiceResponceDto
    {
        public List<InvoiceListTemp> ServicesInvoiceListReadDto { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}