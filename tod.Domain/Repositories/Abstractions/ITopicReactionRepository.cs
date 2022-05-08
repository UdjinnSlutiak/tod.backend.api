using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;

namespace Tod.Domain.Repositories.Abstractions
{
	public interface ITopicReactionRepository : IRepository<TopicReaction>
	{
		public List<int> GetByTopicId(int topicId);
	}
}

