using System.Collections.Generic;

namespace Tod.Services.Requests
{
	public class TopicSearchRequest
	{
		public string Title { get; set; }
        public string Author { get; set; }
        public List<string> Tags { get; set; }
    }
}

