using System;
namespace Tod.Services.Exceptions
{
	public class TopicAlreadyInFavoritesException : ServiceException
	{
		public TopicAlreadyInFavoritesException()
			: base("Topic is already in your favorites") { }
	}
}

