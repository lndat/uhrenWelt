using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using uhrenWelt.Data;

namespace uhrenWelt.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        [Display(Name = "Anzahl")]
        public int Amount { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal NetUnitPrice { get; set; }

        public decimal TaxRate { get; set; }
        public string ManufacturerName { get; set; }
        public string ProductName { get; set; }
        public string ImagePath { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal PriceTotal { get; set; }
    }
}