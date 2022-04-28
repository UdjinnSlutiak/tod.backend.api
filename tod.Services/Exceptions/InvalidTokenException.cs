using System;
namespace Tod.Services.Exceptions
{
	public class InvalidTokenException : ServiceException
	{
		public InvalidTokenException()
			: base("Invalid auth token.")
		{
		}
	}
}

