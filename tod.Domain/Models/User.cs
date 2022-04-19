using Tod.Domain.Models.Enums;

namespace Tod.Domain.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Username { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string PasswordHash { get; set; }
        public double Rating { get; set; }
        public string PhotoUrl { get; set; }
        public ContentStatus Status { get; set; }
    }
}
