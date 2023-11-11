using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.ViewModels
{
    public class CustomerCarsInfoReadDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerTcNo { get; set; }
        public string AracCinsiAdi { get; set; }
        public string YakitTuruAdi { get; set; }
        public int CustomerCarsId { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string ModelYear { get; set; }
        public string Kilometer { get; set; }
        public string Plaka { get; set; }
    }
}