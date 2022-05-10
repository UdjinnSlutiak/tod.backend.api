using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tod.Domain.Models;
using Tod.Domain.Repositories.Abstractions;
using Tod.Domain.Repositories.Realisations.Sql;

namespace Tod.Domain.Repositories.Implementations.Sql
{
	public class SqlUserTopicReactionRepository : SqlBaseRepository<UserTopicReaction>, IUserTopicReactionRepository
	{
		private readonly ProjectContext context;

		public SqlUserTopicReactionRepository(ProjectContext context)
			: base(context)
		{
			this.context = context;
		}

        public List<int> GetByTopicId(int topicId)
        {
			return context.Set<UserTopicReaction>().Where(tr => tr.TopicId == topicId)
				.Select(tr => tr.ReactionId).ToList();
        }

        public async Task<int> GetByUserIdAndTopicId(int userId, int topicId)
        {
			return await context.Set<UserTopicReaction>().Where(utr => utr.UserId == userId && utr.TopicId == topicId)
				.Select(utr => utr.ReactionId).FirstOrDefaultAsync();
        }

		public async Task DeleteByReactionId(int reactionId)
		{
			var record = await this.context.Set<UserTopicReaction>().FirstOrDefaultAsync(utr => utr.ReactionId == reactionId);
			this.context.Set<UserTopicReaction>().Remove(record);
		}
	}
}

