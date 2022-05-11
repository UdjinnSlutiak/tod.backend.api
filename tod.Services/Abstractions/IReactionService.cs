using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Domain.Models.Enums;
using Tod.Services.Requests;
using Tod.Services.Responses;

namespace Tod.Services.Abstractions
{
	public interface IReactionService
	{
		public Task<List<Reaction>> GetByTopicIdAsync(int topicId);
		public Task<List<Reaction>> GetByCommentaryIdAsync(int commentaryId);
		public Task<bool> ReactOnTopicAsync(int topicId, int userId, ReactionValue value);
		public Task<bool> ReactOnCommentaryAsync(int commentaryId, int userId, ReactionValue value);
		public Task<ContentReactionData> GetUserTopicReactionByTopicId(int userId, int topicId);
		public Task<List<ContentReactionData>> GetUserCommentariesReactions(int userId, int topicId);
	}
}

