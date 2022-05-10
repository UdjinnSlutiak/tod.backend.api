using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;

namespace Tod.Domain.Repositories.Abstractions
{
	public interface IUserTopicReactionRepository : IRepository<UserTopicReaction>
	{
		public List<int> GetByTopicId(int topicId);
		public Task<int> GetByUserIdAndTopicId(int userId, int topicId);
		public Task DeleteByReactionId(int reactionId);
	}
}

