using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;

namespace Tod.Domain.Repositories.Abstractions
{
	public interface IUserCommentaryRepository : IRepository<UserCommentary>
	{
		public Task<int> GetUserIdByCommentaryId(int commentaryId);
		public List<int> GetCommentariesIdsByUserId(int userId);
	}
}

