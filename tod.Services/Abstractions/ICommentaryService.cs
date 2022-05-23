using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Services.Requests;
using Tod.Services.Responses;

namespace Tod.Services.Abstractions
{
	public interface ICommentaryService
	{
		public Task<Commentary> GetByIdAsync(int id);
		public Task<GetCommentariesResponse> GetTopicCommentariesAsync(int topicId);
		public Task<List<int>> GetLatestDiscussedTopicsIds(int userId, int count = 20);
		public Task<CommentaryData> CreateCommentaryAsync(int topicId, int userId, string text);
		public Task<CommentaryData> UpdateCommentaryAsync(int userId, int commentaryId, UpdateCommentaryRequest request);
		public Task MarkCommentaryDeletedAsync(int userId, int commentaryId);
	}
}

