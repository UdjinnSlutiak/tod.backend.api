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
	public class SqlUserCommentaryReactionRepository : SqlBaseRepository<UserCommentaryReaction>, IUserCommentaryReactionRepository
	{
        private readonly ProjectContext context;

		public SqlUserCommentaryReactionRepository(ProjectContext context)
            : base(context)
		{
            this.context = context;
		}

        public List<int> GetByCommentaryId(int commentaryId)
        {
            return context.Set<UserCommentaryReaction>().Where(cr => cr.CommentaryId == commentaryId)
                .Select(cr => cr.ReactionId).ToList();
        }

        public async Task<int> GetByUserIdAndCommentaryId(int userId, int commentaryId)
        {
            return await this.context.Set<UserCommentaryReaction>().Where(ucr => ucr.UserId == userId && ucr.CommentaryId == commentaryId)
                .Select(ucr => ucr.ReactionId).FirstOrDefaultAsync();
        }

        public async Task DeleteByReactionId(int reactionId)
        {
            var record = await this.context.Set<UserCommentaryReaction>().FirstOrDefaultAsync(ucr => ucr.ReactionId == reactionId);
            this.context.Set<UserCommentaryReaction>().Remove(record);
        }
    }
}

