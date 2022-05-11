using System;
using Tod.Domain.Models.Enums;

namespace Tod.Services.Exceptions
{
	public class AlreadyReactedException : ServiceException
	{
		public AlreadyReactedException(ContentType contentType, ReactionValue value)
			: base($"You have already " +
                  $"{(value == ReactionValue.Positive ? "liked" : "disliked")}" +
                  $" this " +
                  $"{(contentType == ContentType.Topic ? "topic" : "commentary")}.")
		{ }
	}
}

