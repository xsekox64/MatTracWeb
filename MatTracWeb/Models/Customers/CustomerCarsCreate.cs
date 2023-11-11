using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.Models.Customers
{
    public class CustomerCarsCreate
    {
        public int CustomerId { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string ModelYear { get; set; }
        public string Kilometer { get; set; }
        public string Plaka { get; set; }
        public string SasiNo { get; set; }
        public string MotorNo { get; set; }
        public int AracCinsi { get; set; }
        public int YakitTuru { get; set; }
    }
}