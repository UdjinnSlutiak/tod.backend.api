using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;

namespace Tod.Domain.Repositories.Abstractions
{
	public interface ITopicTagRepository : IRepository<TopicTag>
	{
		public Task<List<int>> GetByTopicIdAsync(int topicId);
		public Task<List<int>> GetByTagIdAsync(int tagId);
	}
}

