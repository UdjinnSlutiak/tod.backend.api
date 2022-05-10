using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;

namespace Tod.Domain.Repositories.Abstractions
{
	public interface IUserCommentaryReactionRepository : IRepository<UserCommentaryReaction>
	{
		public List<int> GetByCommentaryId(int commentaryId);
		public Task<int> GetByUserIdAndCommentaryId(int userId, int commentaryId);
		public Task DeleteByReactionId(int reactionId);
	}
}

