using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebHelper.ModelValidation;

namespace WebHelper.Models
{
    public class Gebruiker
    {
        [DisplayName("Gebruiker ID")]
        public int GebruikerId { get; set; }
        [Required]
        public string Voornaam { get; set; }
        [Required(ErrorMessage = "Dit veld is verplicht.")]
        public string Naam { get; set; }
        [Required, DisplayName("E-mailadres")]
        [EmailValidation]

        public string Email { get; set; }
        public string Functie { get; set; }
        [DisplayName("Naam")]
        public string VolledigeNaam { get { return $"{Voornaam} {Naam}"; } }
    }
}
