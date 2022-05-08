using System;
namespace Tod.Services.Exceptions
{
	public class TopicAlreadyExistsException : ServiceException
	{
		public TopicAlreadyExistsException()
			: base("Topic with such title already exists.") { }
	}
}

