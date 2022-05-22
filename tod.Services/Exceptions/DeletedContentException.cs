using System;
using Tod.Domain.Models.Enums;

namespace Tod.Services.Exceptions
{
	public class DeletedContentException : ServiceException
	{
		public DeletedContentException(ContentType contentType)
			: base($"This {contentType.ToString().ToLower()} was deleted by its owner.")
		{
		}
	}
}

