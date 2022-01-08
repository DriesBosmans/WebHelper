using System.ComponentModel.DataAnnotations;

namespace WEB_voorbereiding.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        [Required]
        public int GebruikerId { get; set; } 
        public Gebruiker Gebruiker { get; set;}

    }
}
