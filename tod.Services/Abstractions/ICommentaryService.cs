﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Services.Responses;

namespace Tod.Services.Abstractions
{
	public interface ICommentaryService
	{
		public Task<Commentary> GetByIdAsync(int id);
		public Task<GetCommentariesResponse> GetCommentariesAsync(int topicId);
		public Task<CommentaryData> CreateCommentaryAsync(int topicId, int userId, string text);
	}
}

