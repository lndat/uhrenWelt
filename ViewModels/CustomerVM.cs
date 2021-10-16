using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace uhrenWelt.ViewModels
{
    public class CustomerVM
    {
        public int Id { get; set; }

        [Display(Name = "Titel")] 
        public string Title { get; set; }

        [Required(ErrorMessage = "Pflichtfeld!")]
        [Display(Name = "Vorname")] 
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Pflichtfeld!")]
        [Display(Name = "Nachname")] 
        public string LastName { get; set; }

        [Required(ErrorMessage = "Pflichtfeld!")]
        [Display(Name = "Email-Adresse")] 
        public string Email { get; set; }

        [Required(ErrorMessage = "Pflichtfeld!")]
        [Display(Name = "Straße")] 
        public string Street { get; set; }

        [Required(ErrorMessage = "Pflichtfeld!")]
        [Display(Name = "Postleitzahl")] 
        public string Zip { get; set; }

        [Required(ErrorMessage = "Pflichtfeld!")]
        [Display(Name = "Stadt")] 
        public string City { get; set; }

        [Required(ErrorMessage = "Pflichtfeld!")]
        [Display(Name = "Passwort")] 
        public string PwHash { get; set; }

        [Required(ErrorMessage = "Pflichtfeld!")]
        [Display(Name = "Passwort wiederholen")]
        public string PwCheck { get; set; }

        public string Salt { get; set; }
    }
}