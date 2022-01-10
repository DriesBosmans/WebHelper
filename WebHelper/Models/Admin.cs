using System.ComponentModel.DataAnnotations;

namespace WebHelper.Models
{
    public class Admin
    {
        public int AdminId { get; set; }  
        [Required]
        public int GebruikerId { get; set; }
        public Gebruiker Gebruiker { get; set; }

    }
}
