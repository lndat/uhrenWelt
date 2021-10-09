using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uhrenWelt.ViewModel
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal NetUnitPrice { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public string ManufacturerId { get; set; }
        public int CategoryId { get; set; }
    }
}