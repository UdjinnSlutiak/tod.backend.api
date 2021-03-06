using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Domain.Models.Enums;
using Tod.Domain.Repositories.Abstractions;
using Tod.Services.Abstractions;
using Tod.Services.Exceptions;
using Tod.Services.Requests;
using Tod.Services.Responses;

namespace Tod.Services.Implementations
{
	public class ReactionService : IReactionService
	{
        private readonly IReactionRepository reactionRepository;
        private readonly IUserTopicReactionRepository userTopicReactionRepository;
        private readonly IUserCommentaryReactionRepository userCommentaryReactionRepository;
        private readonly ITopicRepository topicRepository;
        private readonly ICommentaryRepository commentaryRepository;
        private readonly IUserTopicRepository userTopicRepository;
        private readonly IUserCommentaryRepository userCommentaryRepository;
        private readonly ITopicCommentaryRepository topicCommentaryRepository;
        private readonly IUserService userService;
        private readonly IContentValidator contentValidator;

        public ReactionService(IUserTopicReactionRepository userTopicReactionRepository,
            IReactionRepository reactionRepository,
            IUserCommentaryReactionRepository userCommentaryReactionRepository,
            ITopicRepository topicRepository,
            ICommentaryRepository commentaryRepository,
            IUserTopicRepository userTopicRepository,
            IUserCommentaryRepository userCommentaryRepository,
            ITopicCommentaryRepository topicCommentaryRepository,
            IUserService userService,
            IContentValidator contentValidator)
		{
            this.reactionRepository = reactionRepository;
            this.userTopicReactionRepository = userTopicReactionRepository;
            this.userCommentaryReactionRepository = userCommentaryReactionRepository;
            this.topicRepository = topicRepository;
            this.commentaryRepository = commentaryRepository;
            this.userTopicRepository = userTopicRepository;
            this.userCommentaryRepository = userCommentaryRepository;
            this.topicCommentaryRepository = topicCommentaryRepository;
            this.userService = userService;
            this.contentValidator = contentValidator;
        }

        public async Task<List<Reaction>> GetByTopicIdAsync(int topicId)
        {
            var reactionsIds = this.userTopicReactionRepository.GetByTopicId(topicId);

            var reactions = new List<Reaction>();
            foreach (var reactionId in reactionsIds)
            {
                var reaction = await this.reactionRepository.GetAsync(reactionId);
                reactions.Add(reaction);
            }

            return reactions;
        }

        public async Task<List<Reaction>> GetReactionsByCommentaryIdAsync(int commentaryId)
        {
            var reactionsIds = this.userCommentaryReactionRepository.GetByCommentaryId(commentaryId);

            var reactions = new List<Reaction>();
            foreach (var reactionId in reactionsIds)
            {
                var reaction = await this.reactionRepository.GetAsync(reactionId);
                reactions.Add(reaction);
            }

            return reactions;
        }

        public async Task<bool> ReactOnTopicAsync(int topicId, int userId, ReactionValue value)
        {
            var topic = await this.contentValidator.GetAndValidateTopicAsync(topicId);

            var user = await this.contentValidator.GetAndValidateUserAsync(userId);

            var authorId = await this.userTopicRepository.GetUserIdByTopicIdAsync(topicId);

            if (authorId == userId)
            {
                throw new ContentBelongsToYouException();
            }

            var reactionId = await this.userTopicReactionRepository.GetByUserIdAndTopicId(userId, topicId);

            if (reactionId == 0)
            {
                var newReaction = new Reaction
                {
                    ReactionValue = value,
                    CreatedUtc = DateTime.UtcNow
                };

                newReaction = await this.reactionRepository.CreateAsync(newReaction);
                await this.CreateUserTopicReactionAsync(userId, topicId, newReaction.Id);
                await this.UpdateAuthorRating(topicId, authorId, (int)value, ContentType.Topic);

                return true;
            }

            var reaction = await this.reactionRepository.GetAsync(reactionId);

            if (reaction.ReactionValue == value)
            {
                throw new AlreadyReactedException(ContentType.Topic, value);
            }

            reaction.ReactionValue = value;
            await this.reactionRepository.UpdateAsync(reaction);
            await this.UpdateAuthorRating(topicId, authorId, (int)value * 2, ContentType.Topic);

            return true;
        }

        public async Task<bool> ReactOnCommentaryAsync(int commentaryId, int userId, ReactionValue value)
        {
            var commentary = await this.contentValidator.GetAndValidateCommentaryAsync(commentaryId);

            var user = await this.contentValidator.GetAndValidateUserAsync(userId);

            var authorId = await this.userCommentaryRepository.GetUserIdByCommentaryId(commentaryId);

            if (authorId == userId)
            {
                return false;
            }

            var reactionId = await this.userCommentaryReactionRepository.GetByUserIdAndCommentaryId(userId, commentaryId);

            if (reactionId == 0)
            {
                var newReaction = new Reaction
                {
                    ReactionValue = value,
                    CreatedUtc = DateTime.UtcNow
                };

                newReaction = await this.reactionRepository.CreateAsync(newReaction);
                await this.CreateUserCommentaryReactionAsync(userId, commentaryId, newReaction.Id);

                await this.UpdateAuthorRating(commentaryId, authorId, (int)value, ContentType.Commentary);

                return true;
            }

            var reaction = await this.reactionRepository.GetAsync(reactionId);

            if (reaction.ReactionValue == value)
            {
                throw new AlreadyReactedException(ContentType.Commentary, value);
            }

            reaction.ReactionValue = value;
            await this.reactionRepository.UpdateAsync(reaction);
            await this.UpdateAuthorRating(commentaryId, authorId, (int)value * 2, ContentType.Commentary);

            return true;
        }

        public async Task<ContentReactionData> GetUserTopicReactionByTopicId(int userId, int topicId)
        {
            var user = await this.contentValidator.GetAndValidateUserAsync(userId);

            var topic = await this.contentValidator.GetAndValidateTopicAsync(topicId);

            var reactionId = await this.userTopicReactionRepository.GetByUserIdAndTopicId(userId, topicId);

            if (reactionId == 0)
            {
                return new ContentReactionData
                {
                    Id = topicId,
                    Reacted = false,
                    ReactedPositive = false
                };
            }

            var reaction = await this.reactionRepository.GetAsync(reactionId);

            if (reaction == null)
            {
                throw new NotFoundException(ContentType.Reaction);
            }

            return new ContentReactionData
            {
                Id = topicId,
                Reacted = true,
                ReactedPositive = reaction.ReactionValue == ReactionValue.Positive
            };
        }

        public async Task<ContentReactionsResponse> GetUserCommentariesReactions(int userId, int topicId)
        {
            var user = await this.contentValidator.GetAndValidateUserAsync(userId);

            var topic = await this.contentValidator.GetAndValidateTopicAsync(topicId);

            var commentariesIds = await this.topicCommentaryRepository.GetCommentariesIdByTopicIdAsync(topicId);

            var contentReactionsData = new List<ContentReactionData>();
            foreach (var commentaryId in commentariesIds)
            {
                var reactionId = await this.userCommentaryReactionRepository.GetByUserIdAndCommentaryId(userId, commentaryId);
                if (reactionId == 0)
                {
                    continue;
                }

                var reaction = await this.reactionRepository.GetAsync(reactionId);

                var contentReactionData = new ContentReactionData
                {
                    Id = commentaryId,
                    Reacted = true,
                    ReactedPositive = reaction.ReactionValue == ReactionValue.Positive
                };

                contentReactionsData.Add(contentReactionData);
            }

            return new ContentReactionsResponse
            {
                Reactions = contentReactionsData
            };
        }

        public async Task<ContentReactionData> GetUserCommentaryReaction(int userId, int commentaryId)
        {
            var user = await this.contentValidator.GetAndValidateUserAsync(userId);

            var commentary = await this.contentValidator.GetAndValidateCommentaryAsync(commentaryId);

            var reactionId = await this.userCommentaryReactionRepository.GetByUserIdAndCommentaryId(userId, commentaryId);

            if (reactionId == 0)
            {
                return new ContentReactionData
                {
                    Id = commentaryId,
                    Reacted = false,
                    ReactedPositive = false
                };
            }

            var reaction = await this.reactionRepository.GetAsync(reactionId);

            if (reaction == null)
            {
                throw new NotFoundException(ContentType.Reaction);
            }

            return new ContentReactionData
            {
                Id = commentaryId,
                Reacted = true,
                ReactedPositive = reaction.ReactionValue == ReactionValue.Positive
            };
        }

        private async Task UpdateAuthorRating(int contentId, int authorId, int value, ContentType contentType)
        {
            var author = await this.userService.GetByIdAsync(authorId);
            author.Rating += contentType == ContentType.Topic ? value * 0.5 : value * 0.1;
            await this.userService.UpdateAsync(author);
        }

        private async Task CreateUserCommentaryReactionAsync(int userId, int commentaryId, int ReactionId)
        {
            var ucr = new UserCommentaryReaction
            {
                UserId = userId,
                CommentaryId = commentaryId,
                ReactionId = ReactionId
            };

            await this.userCommentaryReactionRepository.CreateAsync(ucr);
        }

        private async Task CreateUserTopicReactionAsync(int userId, int topicId, int ReactionId)
        {
            var utr = new UserTopicReaction
            {
                UserId = userId,
                TopicId = topicId,
                ReactionId = ReactionId
            };

            await this.userTopicReactionRepository.CreateAsync(utr);
        }
    }
}

