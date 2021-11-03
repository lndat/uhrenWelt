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

        [DataType(DataType.PostalCode)]
        [Range(1010, 9992, ErrorMessage = "Geben Sie eine gültige Postleitzahl ein.")]
        [Required(ErrorMessage = "Pflichtfeld!")]
        [Display(Name = "Postleitzahl")] 
        public string Zip { get; set; }

        [StringLength(200)]
        [Required(ErrorMessage = "Pflichtfeld!")]
        [Display(Name = "Stadt")] 
        public string City { get; set; }

        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$", ErrorMessage = "Ihr Passwort muss mindestens 8 Zeichen lang sein und muss mindestens einen Großbuchstaben, einen Kleinbuchstaben und eine Ziffer enthalten.")]
        [DataType(DataType.Password)]
        [StringLength(250)]
        [Required(ErrorMessage = "Pflichtfeld!")]
        [Display(Name = "Passwort")] 
        public string PwHash { get; set; }

        [DataType(DataType.Password)]
        [Compare("PwHash", ErrorMessage = "Passwort muss übereinstimmen!")]
        [Display(Name = "Passwort wiederholen")]
        public string PwCheck { get; set; }

        public string Salt { get; set; }

        [Display(Name = "Hiermit bestätige ich, die AGB gelesen und anerkannt zu haben.")]
        public bool AcceptTerms { get; set; } = false;
    }




}