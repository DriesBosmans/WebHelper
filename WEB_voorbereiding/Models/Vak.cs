﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEB_voorbereiding.Models
{
    public class Vak
    {
        [Required]
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
