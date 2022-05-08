using System;
using System.Collections.Generic;
using Tod.Domain.Models;

namespace Tod.Domain.Repositories.Abstractions
{
	public interface ICommentaryReactionRepository : IRepository<CommentaryReaction>
	{
		public List<int> GetByCommentaryId(int commentaryId);
	}
}

