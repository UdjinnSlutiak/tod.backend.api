using System;
namespace Tod.Services.Exceptions
{
	public class UsernameAlreadyTakenException : ServiceException
	{
		public UsernameAlreadyTakenException()
			: base("Username is already taken") { }
	}
}

