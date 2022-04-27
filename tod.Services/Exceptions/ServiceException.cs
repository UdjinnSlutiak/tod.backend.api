using System;
namespace Tod.Services.Exceptions
{
	public class ServiceException : Exception
	{
		public ServiceException(string message)
			: base(message) { }
	}
}

