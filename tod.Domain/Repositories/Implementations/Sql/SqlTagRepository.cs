using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tod.Domain.Models;
using Tod.Domain.Repositories.Abstractions;

namespace Tod.Domain.Repositories.Realisations.Sql
{
	public class SqlTagRepository : SqlBaseRepository<Tag>, ITagRepository
	{
		private readonly ProjectContext context;

		public SqlTagRepository(ProjectContext context)
			: base(context)
		{
			this.context = context;
		}

        public async Task<Tag> GetByTitleAsync(string text)
        {
			return await this.context.Set<Tag>().FirstOrDefaultAsync(t => t.Text == text);
        }
    }
}

