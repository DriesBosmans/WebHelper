using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebHelper.Models
{
    public class Vak
    {
        [Required]
        [DisplayName("Cursus")]
        public int VakId { get; set; }
        [Required]
        public string Cursus { get; set; }
        [Required]
        public int StudiePunten { get; set; }
        [Required]
        public string Handboek { get; set; }
        public ICollection<VakLector> VakLectoren { get; set; }
    }
}
