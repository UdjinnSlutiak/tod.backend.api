using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Services.Responses;

namespace Tod.Services.Abstractions
{
	public interface IInterestTagService
	{
		public Task<InterestTagsResponse> GetUserInterestTagsAsync(int userId);
		public Task<InterestTagsResponse> UpdateUserInterestTagsAsync(int userId, List<string> tagsText);
	}
}

