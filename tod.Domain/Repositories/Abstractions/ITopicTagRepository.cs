using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;

namespace Tod.Domain.Repositories.Abstractions
{
	public interface ITopicTagRepository : IRepository<TopicTag>
	{
		public List<int> GetByTopicId(int topicId);
	}
}

