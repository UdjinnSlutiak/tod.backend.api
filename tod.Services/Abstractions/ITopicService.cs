using System;
using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Services.Requests;
using Tod.Services.Responses;

namespace Tod.Services.Abstractions
{
	public interface ITopicService
	{
		public Task<Topic> GetByIdAsync(int topicId);
		public Task<TopicData> GetTopicByIdAsync(int userId, int id);
		public Task<GetTopicsResponse> GetTopicsAsync(int userId, int skip = 0, int offset = 20);
		public Task<GetTopicsResponse> GetFavoritesAsync(int userId, int skip = 0, int offset = 20);
		public Task<GetTopicsResponse> GetMyTopicsAsync(int userId, int skip = 0, int offset = 20);
		public Task<GetTopicsResponse> GetDiscussedTopicsAsync(int userId, int skip = 0, int offset = 20);
		public Task<GetTopicsResponse> GetRecommendedTopicsAsync(int userId, int skip = 0, int offset = 20);
		public Task<CreateTopicResponse> CreateAsync(CreateTopicRequest request, int userId);
		public Task<bool> AddToFavoritesAsync(int topicId, int userId);
		public Task<GetTopicsResponse> SearchTopicsAsync(TopicSearchRequest request);
		public Task MarkTopicDeletedAsync(int userId, int topicId);
	}
}

