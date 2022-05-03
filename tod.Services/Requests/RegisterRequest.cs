using System;
using System.ComponentModel.DataAnnotations;

namespace Tod.Services.Requests
{
	public class RegisterRequest
	{
        [Required(ErrorMessage = "Username is required")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Username must contain at least 4 characters to 30 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Email must contain at least 5 characters to 30 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must contain at least 6 characters to 20 characters.")]
        public string Password { get; set; }
    }
}

