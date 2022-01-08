using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WEB_voorbereiding.Models
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
        [EmailAddress(ErrorMessage = "Onjuist e-mailadres")]
        public string Email { get; set; }

    }
}
