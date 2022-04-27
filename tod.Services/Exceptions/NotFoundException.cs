using System;
namespace Tod.Services.Exceptions
{
	public class NotFoundException : ServiceException
	{
		public NotFoundException(string itemName = null)
			: base($"{itemName ?? "Item"} not found.") { }
	}
}

