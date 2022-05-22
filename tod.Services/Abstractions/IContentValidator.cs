using System;
using System.Threading.Tasks;
using Tod.Domain.Models;

namespace Tod.Services.Abstractions
{
	public interface IContentValidator
	{
		public Task<User> GetAndValidateUserAsync(int userId);
		public Task<Topic> GetAndValidateTopicAsync(int topicId);
		public Task<Commentary> GetAndValidateCommentaryAsync(int commentaryId);
	}
}

