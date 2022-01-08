using System;
using System.Collections.Generic;
using WEB_voorbereiding.Models;

namespace WEB_voorbereiding.ViewModels
{
    public class RegisterResultViewModel
    {
        public bool Succeeded { get; set; }
        public CustomIdentityUser User { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<string> Errors = new List<string>();
    }
}
