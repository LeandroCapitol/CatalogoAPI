﻿using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs
{
    public class LogginModelDTO
    {
        [Required(ErrorMessage = "User name is required")]
        public string? UserName { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
