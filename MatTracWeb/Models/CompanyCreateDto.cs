using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.Models
{
    public class CompanyCreateDto
    {
        public string CompanyName { get; set; }
        public string CompanyAdress { get; set; }
        public string CompanyCategories { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyCounty { get; set; }
        public string ServiceDescription { get; set; }
        public string FaxNumber { get; set; }
        public string LandPhone { get; set; }
        public string CellPhone { get; set; }
    }
}