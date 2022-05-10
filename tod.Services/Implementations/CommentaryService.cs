using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Domain.Models.Dtos;
using Tod.Domain.Models.Enums;
using Tod.Domain.Repositories.Abstractions;
using Tod.Services.Abstractions;
using Tod.Services.Exceptions;
using Tod.Services.Responses;

namespace Tod.Services.Implementations
{
	public class CommentaryService : ICommentaryService
	{
        private readonly ICommentaryRepository commentaryRepository;
        private readonly ITopicCommentaryRepository topicCommentaryRepository;
        private readonly IUserCommentaryRepository userCommentaryRepository;
        private readonly IUserService userService;
        private readonly ITopicService topicService;
        private readonly IReactionService reactionService;

		public CommentaryService(ICommentaryRepository commentaryRepository,
            ITopicCommentaryRepository topicCommentaryRepository,
            IUserCommentaryRepository userCommentaryRepository,
            IUserService userService,
            ITopicService topicService,
            IReactionService reactionService)
		{
            this.commentaryRepository = commentaryRepository;
            this.topicCommentaryRepository = topicCommentaryRepository;
            this.userCommentaryRepository = userCommentaryRepository;
            this.userService = userService;
            this.topicService = topicService;
            this.reactionService = reactionService;
		}

        public async Task<Commentary> GetByIdAsync(int id)
        {
            return await this.commentaryRepository.GetAsync(id);
        }

        public async Task<GetCommentariesResponse> GetCommentariesAsync(int topicId)
        {
            var commentariesIds = this.topicCommentaryRepository.GetCommentariesIdByTopicId(topicId);

            var commentaries = new List<CommentaryData>();
            foreach (var commentaryId in commentariesIds)
            {
                var commentary = await this.commentaryRepository.GetAsync(commentaryId);

                var authorId = await this.userCommentaryRepository.GetUserIdByCommentaryId(commentaryId);

                var user = await this.userService.GetByIdAsync(authorId);

                var reactions = await this.reactionService.GetByCommentaryIdAsync(commentaryId);

                var positive = reactions.Where(r => r.ReactionValue == ReactionValue.Positive).Count();
                var negative = reactions.Count - positive;

                commentaries.Add(new CommentaryData
                {
                    Id = commentaryId,
                    Text = commentary.Text,
                    CreatedUtc = commentary.CreatedUtc,
                    Author = new UserDto(user),
                    Rating = positive - negative
                });
            }

            return new GetCommentariesResponse
            {
                Commentaries = commentaries
            };
        }

        public async Task<CommentaryData> CreateCommentaryAsync(int topicId, int userId, string text)
        {
            var user = await this.userService.GetByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("User");
            }

            if (user.Status == ContentStatus.Banned)
            {
                throw new BannedContentException("author");
            }

            var topic = await this.topicService.GetByIdAsync(topicId);

            if (topic == null)
            {
                throw new NotFoundException("Topic");
            }

            if (topic.Status == ContentStatus.Banned)
            {
                throw new BannedContentException("topic");
            }

            var commentary = new Commentary
            {
                Text = text,
                CreatedUtc = DateTime.UtcNow,
                Status = ContentStatus.Ok
            };

            commentary = await this.commentaryRepository.CreateAsync(commentary);

            await this.CreateUserCommentaryAsync(userId, commentary.Id);
            await this.CreateTopicCommentaryAsync(topicId, commentary.Id);

            return new CommentaryData
            {
                Id = commentary.Id,
                Text = commentary.Text,
                CreatedUtc = commentary.CreatedUtc,
                Author = new UserDto(user)
            };
        }

        private async Task CreateUserCommentaryAsync(int userId, int commentaryId)
        {
            var userCommentary = new UserCommentary
            {
                UserId = userId,
                CommentaryId = commentaryId
            };

            await this.userCommentaryRepository.CreateAsync(userCommentary);
        }

        private async Task CreateTopicCommentaryAsync(int topicId, int commentaryId)
        {
            var topicCommentary = new TopicCommentary
            {
                TopicId = topicId,
                CommentaryId = commentaryId
            };

            await this.topicCommentaryRepository.CreateAsync(topicCommentary);
        }
    }
}

