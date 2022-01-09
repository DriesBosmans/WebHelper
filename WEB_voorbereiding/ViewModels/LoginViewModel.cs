﻿using System.ComponentModel.DataAnnotations;

namespace WEB_voorbereiding.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        //public int Id { get; set; }
    }
}