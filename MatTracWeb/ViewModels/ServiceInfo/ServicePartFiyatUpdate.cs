using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.ViewModels.ServiceInfo
{
    public class ServicePartFiyatUpdate
    {
        public int ParcaFiyatId { get; set; }
        public string IslemAdi { get; set; }
        public double BirimFiyat { get; set; }
        public string ParaKodu { get; set; }
        public double Iscilik { get; set; }
        public int Amount { get; set; }
        public Guid IdName { get; set; }
    }
}