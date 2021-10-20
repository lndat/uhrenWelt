using System;
using System.Collections.Generic;
using uhrenWelt.Data;

namespace uhrenWelt.Models
{
    public class Orders
    {
        public OrderLine OrderLine { get; set; }
        public decimal PriceWithTax { get; set; }

        public Orders(OrderLine orderLine, decimal priceWithTax)
        {
            OrderLine = orderLine;
            PriceWithTax = priceWithTax;
        }
    }
}