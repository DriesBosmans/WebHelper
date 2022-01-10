using System.Collections.Generic;
using System.ComponentModel;

namespace WEB_voorbereiding.Models
{
    public class Lector
    {
        [DisplayName("Lector")]
        public int LectorId { get; set; }
        public int GebruikerId { get; set; }
        public Gebruiker Gebruiker { get; set;}
        public ICollection<VakLector> VakLectoren { get; set; }
    }
}
