using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace uhrenWelt.ViewModel
{
    public class CustomerTblVM
    {
        public int Id { get; set; }
        [Display(Name = "Titel")] public string Title { get; set; }
        [Display(Name = "Vorname")] public string FirstName { get; set; }
        [Display(Name = "Nachname")] public string LastName { get; set; }
        [Display(Name = "Email-Adresse")] public string Email { get; set; }
        [Display(Name = "Straße")] public string Street { get; set; }
        [Display(Name = "Postleitzahl")] public string Zip { get; set; }
        [Display(Name = "Stadt")] public string City { get; set; }
        public string PwHash { get; set; }
        public string Salt { get; set; }
    }
}