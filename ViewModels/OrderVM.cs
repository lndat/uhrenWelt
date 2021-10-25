using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uhrenWelt.Data;

namespace uhrenWelt.ViewModels
{
    public class OrderVM
    {
        public Customer Customer { get; set; }
        public OrderLine OrderLine { get; set; }
        public Order Order { get; set; }
    }
}