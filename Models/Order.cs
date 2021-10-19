using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uhrenWelt.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal PriceTotal { get; set; }
        public DateTime DateOrdered { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}