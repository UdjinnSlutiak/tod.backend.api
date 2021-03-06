using System;
using System.Collections.Generic;
using Tod.Domain.Models;
using Tod.Domain.Models.Dtos;

namespace Tod.Services.Responses
{
	public class TopicData
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedUtc { get; set; }
        public UserDto Author { get; set; }
        public List<Tag> Tags { get; set; }
        public int Rating { get; set; }
        public bool IsInFavorite { get; set; }
    }
}

