using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace uhrenWelt.ViewModel
{
    public class StatsVM
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)] 
        public decimal NetUnitPrice { get; set; }

        public int ManufacturerId { get; set; }

        public string ManufacturerName { get; set; }

        public int CategoryId { get; set; }

        public int Amount { get; set; } = 1;
    }
}