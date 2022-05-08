using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tod.Domain.Models;
using Tod.Domain.Repositories.Abstractions;
using Tod.Domain.Repositories.Realisations.Sql;

namespace Tod.Domain.Repositories.Implementations.Sql
{
	public class SqlUserTopicRepository : SqlBaseRepository<UserTopic>, IUserTopicRepository
	{
		private readonly ProjectContext context;

		public SqlUserTopicRepository(ProjectContext context)
			: base(context)
		{
			this.context = context;
		}

        public async Task<int> GetUserIdByTopicIdAsync(int topicId)
        {
			return (await context.Set<UserTopic>().FirstOrDefaultAsync(ut => ut.TopicId == topicId)).UserId;
        }
    }
}

