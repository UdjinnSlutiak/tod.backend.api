using System;
using Tod.Domain.Models.Enums;

namespace Tod.Services.Exceptions
{
	public class NotFoundException : ServiceException
	{
		public NotFoundException(ContentType contentType)
			: base($"{contentType} not found.") { }
	}
}

