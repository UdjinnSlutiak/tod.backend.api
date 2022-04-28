using System;
namespace Tod.Services.Exceptions
{
	public class UnableToValidateTokenException : ServiceException
	{
		public UnableToValidateTokenException()
			: base("Unable to validate auth token.")
		{
		}
	}
}

