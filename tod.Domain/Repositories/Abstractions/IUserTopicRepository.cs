using System;
using System.Threading.Tasks;
using Tod.Domain.Models;

namespace Tod.Domain.Repositories.Abstractions
{
	public interface IUserTopicRepository : IRepository<UserTopic>
	{
		public Task<int> GetUserIdByTopicIdAsync(int topicId);
	}
}

