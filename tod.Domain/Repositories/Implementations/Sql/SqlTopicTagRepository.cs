using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Domain.Repositories.Abstractions;
using Tod.Domain.Repositories.Realisations.Sql;

namespace Tod.Domain.Repositories.Implementations.Sql
{
	public class SqlTopicTagRepository : SqlBaseRepository<TopicTag>, ITopicTagRepository
	{
		private readonly ProjectContext context;

		public SqlTopicTagRepository(ProjectContext context)
			: base(context)
		{
			this.context = context;
		}

        public List<int> GetByTopicId(int topicId)
        {
			return this.context.Set<TopicTag>().Where(tt => tt.TopicId == topicId)
				.Select(tt => tt.TagId).ToList();
        }
    }
}

