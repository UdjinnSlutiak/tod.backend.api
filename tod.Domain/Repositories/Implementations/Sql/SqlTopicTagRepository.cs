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
	public class SqlTopicTagRepository : SqlBaseRepository<TopicTag>, ITopicTagRepository
	{
		private readonly ProjectContext context;

		public SqlTopicTagRepository(ProjectContext context)
			: base(context)
		{
			this.context = context;
		}

        public async Task<List<int>> GetByTopicIdAsync(int topicId)
        {
			return await this.context.Set<TopicTag>().Where(tt => tt.TopicId == topicId)
				.Select(tt => tt.TagId).ToListAsync();
        }

		public async Task<List<int>> GetByTagIdAsync(int tagId)
        {
			return await this.context.Set<TopicTag>().Where(tt => tt.TagId == tagId)
				.Select(tt => tt.TopicId).ToListAsync();
        }

	}
}

