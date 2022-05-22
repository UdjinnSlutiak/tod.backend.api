using System;
namespace Tod.Services.Exceptions
{
	public class PermissionDeniedException : ServiceException
	{
		public PermissionDeniedException()
			: base("Permission denied.")
		{
		}
	}
}

