using System.ComponentModel.DataAnnotations;

namespace WebHelper.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        [Required]
        public int GebruikerId { get; set; } 
        public Gebruiker Gebruiker { get; set;}

    }
}
