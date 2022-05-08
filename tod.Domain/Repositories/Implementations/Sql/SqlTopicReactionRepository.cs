using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Domain.Repositories.Abstractions;
using Tod.Domain.Repositories.Realisations.Sql;

namespace Tod.Domain.Repositories.Implementations.Sql
{
	public class SqlTopicReactionRepository : SqlBaseRepository<TopicReaction>, ITopicReactionRepository
	{
		private readonly ProjectContext context;

		public SqlTopicReactionRepository(ProjectContext context)
			: base(context)
		{
			this.context = context;
		}

        public List<int> GetByTopicId(int topicId)
        {
			return context.Set<TopicReaction>().Where(tr => tr.TopicId == topicId).Select(tr => tr.ReactionId).ToList();
        }
    }
}

