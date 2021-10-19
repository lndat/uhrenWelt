using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uhrenWelt.Data;

namespace uhrenWelt.Models
{
    public class Cart
    {
        public Product Product { get; set; }
        public int Amount { get; set; }

        public Cart(Product product, int amount)
        {
            Product = product;
            Amount = amount;
        }
    }
}