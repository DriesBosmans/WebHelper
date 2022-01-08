using System.ComponentModel.DataAnnotations;

namespace WEB_voorbereiding.Models
{
    public class VakLector
    {
        public int VakLectorId { get; set; }
        [Required]
        public int VakId{ get; set; }
        public Vak Vak { get; set; }
        [Required]
        public int LectorId { get; set; } 
        public Lector Lector { get; set; }
    }
}
