using System;
namespace Tod.Services.Exceptions
{
	public class PasswordMismatchException : ServiceException
	{
		public PasswordMismatchException()
			: base("Wrong password.")
		{
		}
	}
}

