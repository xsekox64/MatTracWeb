using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.ViewModels.Services
{
    public class ServicesCustomerCars
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; } 
        public string VergiNo { get; set; }
        public string VergiDairesi { get; set; }
        public int CustomerCarsId { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string ModelYear { get; set; }
        public string Kilometer { get; set; }
        public string Plaka { get; set; }
    }
}