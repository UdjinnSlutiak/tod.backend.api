using System.ComponentModel.DataAnnotations;

namespace Tod.Services.Requests
{
	public class LoginRequest
	{
        [Required(ErrorMessage = "Login is required")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Login must contain at least 4 characters to 30 characters.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must contain at least 6 characters to 20 characters.")]
        public string Password { get; set; }
    }
}

