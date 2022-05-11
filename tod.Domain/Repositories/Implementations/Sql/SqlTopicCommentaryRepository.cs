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
	public class SqlTopicCommentaryRepository : SqlBaseRepository<TopicCommentary>, ITopicCommentaryRepository
	{
        private readonly ProjectContext context;

		public SqlTopicCommentaryRepository(ProjectContext context)
            : base(context)
		{
            this.context = context;
		}

        public async Task<List<int>> GetCommentariesIdByTopicIdAsync(int topicId)
        {
            return await context.Set<TopicCommentary>().Where(tc => tc.TopicId == topicId).Select(tc => tc.CommentaryId).ToListAsync();
        }
    }
}

