using System;
namespace Tod.Services.Exceptions
{
	public class ContentBelongsToYouException : ServiceException
	{
		public ContentBelongsToYouException()
			: base("This content belongs to you.") { }
	}
}

