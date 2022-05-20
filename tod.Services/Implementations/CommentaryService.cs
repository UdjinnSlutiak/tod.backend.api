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
        private readonly ITopicRepository topicRepository;
        private readonly ITopicCommentaryRepository topicCommentaryRepository;
        private readonly IUserCommentaryRepository userCommentaryRepository;
        private readonly IUserService userService;
        private readonly IReactionService reactionService;

		public CommentaryService(ICommentaryRepository commentaryRepository,
            ITopicRepository topicRepository,
            ITopicCommentaryRepository topicCommentaryRepository,
            IUserCommentaryRepository userCommentaryRepository,
            IUserService userService,
            IReactionService reactionService)
		{
            this.commentaryRepository = commentaryRepository;
            this.topicRepository = topicRepository;
            this.topicCommentaryRepository = topicCommentaryRepository;
            this.userCommentaryRepository = userCommentaryRepository;
            this.userService = userService;
            this.reactionService = reactionService;
		}

        public async Task<Commentary> GetByIdAsync(int id)
        {
            return await this.commentaryRepository.GetAsync(id);
        }

        public async Task<GetCommentariesResponse> GetTopicCommentariesAsync(int topicId)
        {
            var commentariesIds = await this.topicCommentaryRepository.GetCommentariesIdByTopicIdAsync(topicId);

            var commentaries = new List<CommentaryData>();
            foreach (var commentaryId in commentariesIds)
            {
                var commentary = await this.commentaryRepository.GetAsync(commentaryId);

                var authorId = await this.userCommentaryRepository.GetUserIdByCommentaryId(commentaryId);

                var user = await this.userService.GetByIdAsync(authorId);

                var reactions = await this.reactionService.GetReactionsByCommentaryIdAsync(commentaryId);

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

        public async Task<List<int>> GetLatestDiscussedTopicsIds(int userId, int count = 20)
        {
            var userCommentariesIds = this.userCommentaryRepository.GetCommentariesIdsByUserId(userId);

            var topicsIds = new List<int>();
            while (topicsIds.Count != count)
            {
                var commentaryId = userCommentariesIds.FirstOrDefault();
                if (commentaryId == 0)
                {
                    return topicsIds;
                }

                var topicId = await this.topicCommentaryRepository.GetTopicIdByCommentaryId(commentaryId);

                if (topicsIds.Contains(topicId))
                {
                    userCommentariesIds.RemoveAt(0);
                    continue;
                }
                topicsIds.Add(topicId);
            }

            return topicsIds;
        }

        public async Task<CommentaryData> CreateCommentaryAsync(int topicId, int userId, string text)
        {
            var user = await this.GetAndValidateUserAsync(userId);

            var topic = await this.GetAndValidateTopicAsync(topicId);

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

        private async Task<User> GetAndValidateUserAsync(int userId)
        {
            var user = await this.userService.GetByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException(ContentType.User);
            }

            if (user.Status == ContentStatus.Banned)
            {
                throw new BannedContentException(ContentType.User);
            }

            return user;
        }

        private async Task<Topic> GetAndValidateTopicAsync(int topicId)
        {
            var topic = await this.topicRepository.GetAsync(topicId);

            if (topic == null)
            {
                throw new NotFoundException(ContentType.Topic);
            }

            if (topic.Status == ContentStatus.Banned)
            {
                throw new BannedContentException(ContentType.Topic);
            }

            return topic;
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

