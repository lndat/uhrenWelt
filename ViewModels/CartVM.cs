using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using uhrenWelt.Data;

namespace uhrenWelt.ViewModels
{
    public class CartVM
    {
        public int Id { get; set; }
        [Display(Name = "Name")] public string ProductName { get; set; }
        [Display(Name = "Netto Preis")] [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)] public decimal NetUnitPrice { get; set; }
        public string ImagePath { get; set; }
        [Display(Name = "Beschreibung")] public string Description { get; set; }
        public int ManufacturerId { get; set; }
        [Display(Name = "Hersteller")] public string ManufacturerName { get; set; }
        public int CategoryId { get; set; }
        public int Amount { get; set; }
        public Product Product { get; set; }
    }
}