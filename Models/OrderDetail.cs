using System.ComponentModel.DataAnnotations;

namespace uhrenWelt.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int ManufacturerId { get; set; }
        public int Amount { get; set; }
        public int CustomerId { get; set; }
        public decimal NetUnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal PriceTotal { get; set; }
        public string ProductName { get; set; }
        public string ImagePath { get; set; }
        public string ManufacturerName { get; set; }
    }
}