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

        public List<int> GetCommentariesIdsByUserId(int userId)
        {
            return this.context.Set<UserCommentary>().OrderByDescending(uc => uc.CommentaryId).AsEnumerable().Where(uc => uc.UserId == userId).Select(uc => uc.CommentaryId).ToList();
        }
    }
}

