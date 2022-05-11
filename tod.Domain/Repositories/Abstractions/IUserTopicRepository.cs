using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;

namespace Tod.Domain.Repositories.Abstractions
{
	public interface IUserTopicRepository : IRepository<UserTopic>
	{
		public Task<int> GetUserIdByTopicIdAsync(int topicId);
		public Task<List<int>> GetTopicsIdByUserId(int userId);
	}
}

