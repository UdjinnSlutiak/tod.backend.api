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
	public class SqlFavoriteRepository : SqlBaseRepository<FavoriteTopic>, IFavoriteRepository
	{
		private readonly ProjectContext context;

		public SqlFavoriteRepository(ProjectContext context)
			: base(context)
		{
			this.context = context;
		}

        public async Task<FavoriteTopic> GetByUserIdAndTopicId(int userId, int topicId)
        {
			return await this.context.Set<FavoriteTopic>().FirstOrDefaultAsync(f => f.UserId == userId && f.TopicId == topicId);
        }

		public List<int> GetByUserId(int userId)
        {
			return this.context.Set<FavoriteTopic>().OrderByDescending(ft => ft.TopicId).AsEnumerable()
				.Where(f => f.UserId == userId).Select(f => f.TopicId).ToList();
        }
    }
}

