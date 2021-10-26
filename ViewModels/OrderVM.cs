using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uhrenWelt.Data;

namespace uhrenWelt.ViewModels
{
    public class OrderVM
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public decimal PriceTotal { get; set; }
        public decimal NetUnitPrice { get; set; }
        public DateTime DateOrdered { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}