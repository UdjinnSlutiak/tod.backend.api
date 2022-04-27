using System;
namespace Tod.Services.Exceptions
{
	public class EmailAlreadyTakenException : ServiceException
	{
		public EmailAlreadyTakenException()
			: base("Email is already taken.") { }
	}
}

