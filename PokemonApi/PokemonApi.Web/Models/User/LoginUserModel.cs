﻿using System.ComponentModel.DataAnnotations;

namespace PokemonApi.Web.Models.User
{
    public class LoginUserModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
