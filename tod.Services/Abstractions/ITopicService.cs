using System;
using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Services.Requests;
using Tod.Services.Responses;

namespace Tod.Services.Abstractions
{
	public interface ITopicService
	{
		public Task<TopicData> GetTopicByIdAsync(int id);
		public Task<GetTopicsResponse> GetTopicsAsync(int skip, int offset);
		public Task<CreateTopicResponse> CreateAsync(CreateTopicRequest request, int userId);
	}
}

