using System;
using Tod.Domain.Models.Dtos;

namespace Tod.Services.Responses
{
	public class CommentaryData
	{
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedUtc { get; set; }
        public UserDto Author { get; set; }
        public int Rating { get; set; }
    }
}

