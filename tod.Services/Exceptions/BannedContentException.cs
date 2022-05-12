using System;
using Tod.Domain.Models.Enums;

namespace Tod.Services.Exceptions
{
	public class BannedContentException : ServiceException
	{
		public BannedContentException(ContentType contentType)
			: base($"This {contentType.ToString().ToLower()} was banned.") { }
	}
}

