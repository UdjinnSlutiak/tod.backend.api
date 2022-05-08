using System.Collections.Generic;

namespace Tod.Services.Requests
{
	public class CreateTopicRequest
	{
        public string Title { get; set; }
        public List<string> Tags { get; set; }
    }
}

