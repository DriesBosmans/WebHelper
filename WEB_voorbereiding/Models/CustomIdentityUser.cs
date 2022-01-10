using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebHelper.Models
{
    public class CustomIdentityUser : IdentityUser
    {
        public int CustomIdentityUserId { get; set; }  
        [Required]
        public int GebruikerId { get; set; }    
        public Gebruiker Gebruiker { get; set; }
   
    }
}
