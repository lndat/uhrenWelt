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

        [StringLength(150)]
        [Display(Name = "Titel")] 
        public string Title { get; set; }

        [StringLength(150)]
        [Required(ErrorMessage = "Pflichtfeld!")]
        [Display(Name = "Vorname")] 
        public string FirstName { get; set; }

        [StringLength(150)]
        [Required(ErrorMessage = "Pflichtfeld!")]
        [Display(Name = "Nachname")] 
        public string LastName { get; set; }

        [StringLength(250)]
        [EmailAddress]
        [Required(ErrorMessage = "Pflichtfeld!")]
        [Display(Name = "Email-Adresse")] 
        public string Email { get; set; }

        [StringLength(150)]
        [Required(ErrorMessage = "Pflichtfeld!")]
        [Display(Name = "Straße")] 
        public string Street { get; set; }

        [StringLength(4)]
        [Required(ErrorMessage = "Pflichtfeld!")]
        [Display(Name = "Postleitzahl")] 
        public string Zip { get; set; }

        [StringLength(200)]
        [Required(ErrorMessage = "Pflichtfeld!")]
        [Display(Name = "Stadt")] 
        public string City { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Pflichtfeld!")]
        [Display(Name = "Passwort")] 
        public string PwHash { get; set; }

        [Compare("PwHash", ErrorMessage = "Passwort muss übereinstimmen!")]
        [Display(Name = "Passwort")]
        public string PwCheck { get; set; }

        public string Salt { get; set; }
    }
}