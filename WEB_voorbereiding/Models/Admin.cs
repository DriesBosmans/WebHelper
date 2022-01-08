using System.ComponentModel.DataAnnotations;

namespace WEB_voorbereiding.Models
{
    public class Admin
    {
        public int AdminId { get; set; }  
        [Required]
        public int GebruikerId { get; set; }
        public Gebruiker Gebruiker { get; set; }

    }
}
