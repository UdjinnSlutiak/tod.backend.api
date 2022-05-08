using System;
using System.Collections.Generic;
using Tod.Domain.Models;

namespace Tod.Domain.Repositories.Abstractions
{
	public interface ITopicCommentaryRepository : IRepository<TopicCommentary>
	{
		public List<int> GetCommentariesIdByTopicId(int topicId);
	}
}

