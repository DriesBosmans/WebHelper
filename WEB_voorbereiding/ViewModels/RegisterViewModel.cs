using System.ComponentModel.DataAnnotations;
using WEB_voorbereiding.Tools;

namespace WEB_voorbereiding.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Wachtwoord { get; set; }

        [Required]
        [Compare("Wachtwoord", ErrorMessage = "Paswoord moet overeenkomen")]
        public string Herhaalwachtwoord { get; set; }
        [Required]
        public string Voornaam { get; set; }
        [Required]
        public string Naam { get; set; }
 

    }
}
