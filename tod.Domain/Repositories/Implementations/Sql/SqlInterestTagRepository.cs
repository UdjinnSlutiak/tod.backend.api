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
	public class SqlInterestTagRepository : SqlBaseRepository<InterestTag>, IInterestTagRepository
	{
		private readonly ProjectContext context;

		public SqlInterestTagRepository(ProjectContext context)
			: base(context)
		{
			this.context = context;
		}

		public async Task<List<InterestTag>> GetInterestTagsByUserIdAsync(int userId)
        {
			return await this.context.Set<InterestTag>()
				.Where(it => it.UserId == userId).ToListAsync();
        }

		public async Task DeleteUserInterestTagsAsync(int userId)
        {
			var interestTags = await this.context.Set<InterestTag>()
				.Where(it => it.UserId == userId).ToListAsync();

			if (interestTags == null)
            {
				return;
            }

			this.context.Set<InterestTag>().RemoveRange(interestTags);
			await this.context.SaveChangesAsync();
        }
	}
}

