using System;
using System.Collections.Generic;
using System.Linq;
using Tod.Domain.Models;
using Tod.Domain.Repositories.Abstractions;
using Tod.Domain.Repositories.Realisations.Sql;

namespace Tod.Domain.Repositories.Implementations.Sql
{
	public class SqlCommentaryReactionRepository : SqlBaseRepository<CommentaryReaction>, ICommentaryReactionRepository
	{
        private readonly ProjectContext context;

		public SqlCommentaryReactionRepository(ProjectContext context)
            : base(context)
		{
            this.context = context;
		}

        public List<int> GetByCommentaryId(int commentaryId)
        {
            return context.Set<CommentaryReaction>().Where(cr => cr.CommentaryId == commentaryId)
                .Select(cr => cr.ReactionId).ToList();
        }
    }
}

