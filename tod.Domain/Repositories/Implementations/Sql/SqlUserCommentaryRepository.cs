using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tod.Domain.Models;
using Tod.Domain.Repositories.Abstractions;
using Tod.Domain.Repositories.Realisations.Sql;

namespace Tod.Domain.Repositories.Implementations.Sql
{
	public class SqlUserCommentaryRepository : SqlBaseRepository<UserCommentary>, IUserCommentaryRepository
	{
        private readonly ProjectContext context;

		public SqlUserCommentaryRepository(ProjectContext context)
            : base(context)
		{
            this.context = context;
		}

        public async Task<int> GetUserIdByCommentaryId(int commentaryId)
        {
            return (await this.context.Set<UserCommentary>().FirstOrDefaultAsync(uc => uc.CommentaryId == commentaryId)).UserId;
        }
    }
}

