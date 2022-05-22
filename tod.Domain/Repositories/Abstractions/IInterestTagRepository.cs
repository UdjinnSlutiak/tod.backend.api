using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;

namespace Tod.Domain.Repositories.Abstractions
{
	public interface IInterestTagRepository : IRepository<InterestTag>
	{
		public Task<List<InterestTag>> GetInterestTagsByUserIdAsync(int userId);
		public Task DeleteUserInterestTagsAsync(int userId);
	}
}

