using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace uhrenWelt.ViewModel
{
    public class ProductVM
    {
        public int Id { get; set; }
        [Display(Name = "Name")] public string ProductName { get; set; }
        [Display(Name = "Netto Preis")] public decimal NetUnitPrice { get; set; }
        public string ImagePath { get; set; }
        [Display(Name = "Beschreibung")] public string Description { get; set; }
        public string ManufacturerId { get; set; }
        public int CategoryId { get; set; }
    }
}