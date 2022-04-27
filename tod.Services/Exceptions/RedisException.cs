using System;
namespace Tod.Services.Exceptions
{
	public class RedisException : ServiceException
	{
		public RedisException()
			: base("Error while trying to get to RedisCache.") { }
	}
}

