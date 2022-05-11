using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;

namespace Tod.Domain.Repositories.Abstractions
{
	public interface ITopicCommentaryRepository : IRepository<TopicCommentary>
	{
		public Task<List<int>> GetCommentariesIdByTopicIdAsync(int topicId);
	}
}

