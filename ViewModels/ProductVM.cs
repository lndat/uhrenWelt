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

        [Display(Name = "Name")] 
        public string ProductName { get; set; }

        [Display(Name = "Netto Preis")] 
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)] 
        public decimal NetUnitPrice { get; set; }

        public string ImagePath { get; set; }

        [Display(Name = "Beschreibung")] 
        public string Description { get; set; }

        public int ManufacturerId { get; set; }

        [Display(Name = "Hersteller")] 
        public string ManufacturerName { get; set; }

        public int CategoryId { get; set; }

        [Display(Name = "Anzahl")]
        [Range(1, 9, ErrorMessage = "Maximal 5 Stück pro Bestellung!")]
        public int Amount { get; set; } = 1;

        [Display(Name = "Anzahl Aufrufe")]
        public int ViewCounter { get; set; }

        public int Ratings { get; set; }
        public string Comment { get; set; }

    }
}