using System;
using Tod.Domain.Models.Enums;

namespace Tod.Domain.Models
{
	public class Topic
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedUtc { get; set; }
        public ContentStatus Status { get; set; }
    }
}
