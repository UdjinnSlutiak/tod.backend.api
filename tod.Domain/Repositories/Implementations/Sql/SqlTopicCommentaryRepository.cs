using System;
using System.Collections.Generic;
using System.Linq;
using Tod.Domain.Models;
using Tod.Domain.Repositories.Abstractions;
using Tod.Domain.Repositories.Realisations.Sql;

namespace Tod.Domain.Repositories.Implementations.Sql
{
	public class SqlTopicCommentaryRepository : SqlBaseRepository<TopicCommentary>, ITopicCommentaryRepository
	{
        private readonly ProjectContext context;

		public SqlTopicCommentaryRepository(ProjectContext context)
            : base(context)
		{
            this.context = context;
		}

        public List<int> GetCommentariesIdByTopicId(int topicId)
        {
            return context.Set<TopicCommentary>().Where(tc => tc.TopicId == topicId).Select(tc => tc.CommentaryId).ToList();
        }
    }
}

