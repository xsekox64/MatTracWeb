using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatTracWeb.ViewModels
{
    public class CustomerCarsReadDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime AddDateTime { get; set; }
        public string CustomerTcNo { get; set; }
        public bool Active { get; set; }
        public string VergiNo { get; set; }
        public string VergiDairesi { get; set; }
        public List<Car> cars { get; set; }
    }
    public class Car
    {
        public int CustomerCarsId { get; set; }
        public int CustomerId { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string ModelYear { get; set; }
        public string Kilometer { get; set; }
        public string Plaka { get; set; }
    }
}