using Tod.Domain.Models.Enums;

namespace Tod.Domain.Models
{
	public class Tag
	{
        public int Id { get; set; }
        public string Text { get; set; }
        public int UsedCount { get; set; }
        public int UserId { get; set; }
        public ContentStatus Status { get; set; }
    }
}
