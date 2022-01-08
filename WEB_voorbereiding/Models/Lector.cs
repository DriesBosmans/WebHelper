using System.Collections.Generic;

namespace WEB_voorbereiding.Models
{
    public class Lector
    {
        public int LectorId { get; set; }
        public int GebruikerId { get; set; }
        public Gebruiker Gebruiker { get; set;}
        public ICollection<VakLector> VakLectoren { get; set; }
    }
}
