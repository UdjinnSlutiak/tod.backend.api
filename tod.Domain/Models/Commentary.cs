using System;
using Tod.Domain.Models.Enums;

namespace Tod.Domain.Models
{
	public class Commentary
	{
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public ContentStatus Status { get; set; }
    }
}
