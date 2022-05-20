using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tod.Domain.Models;
using Tod.Domain.Models.Enums;
using Tod.Domain.Repositories.Abstractions;

namespace Tod.Domain.Repositories.Realisations.Sql
{
	public class SqlTopicRepository : SqlBaseRepository<Topic>, ITopicRepository
	{
		private readonly ProjectContext context;

		public SqlTopicRepository(ProjectContext context)
			: base(context)
		{
			this.context = context;
		}

        public Task<Topic> GetByTitleAsync(string title)
        {
			return this.context.Set<Topic>().FirstOrDefaultAsync(t => t.Title == title);
        }

		public async Task<List<Topic>> GetRangeAsync(int skip, int offset)
        {
			var clearTopics = await this.context.Set<Topic>().Where(t => t.Status == ContentStatus.Ok).ToListAsync();
			return clearTopics.OrderByDescending(t => t.Id).AsEnumerable().Skip(skip).Take(offset).ToList();
		}

		public async Task<List<Topic>> GetWhereTitleContainsAsync(string titlePart)
        {
			return await this.context.Set<Topic>().Where(t => t.Title.Contains(titlePart)).ToListAsync();
        }
	}
}

