using System;
namespace Tod.Services.Exceptions
{
	public class BannedContentException : ServiceException
	{
		public BannedContentException(string contentType)
			: base($"This {contentType} was banned.") { }
	}
}

