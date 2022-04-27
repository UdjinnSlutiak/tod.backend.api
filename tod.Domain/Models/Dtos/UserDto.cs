using Tod.Domain.Models.Enums;

namespace Tod.Domain.Models.Dtos
{
	public class UserDto
	{
        public UserDto(User user)
        {
            this.Username = user.Username;
            this.Email = user.Email;
            this.Role = user.Role;
            this.Rating = user.Rating;
            this.PhotoUrl = user.PhotoUrl;
            this.Status = user.Status;
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public double Rating { get; set; }
        public string PhotoUrl { get; set; }
        public ContentStatus Status { get; set; }
    }
}

