using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;

namespace Tod.Services.Abstractions
{
	public interface IReactionService
	{
		public Task<List<Reaction>> GetReactionsByTopicIdAsync(int topicId);
	}
}

