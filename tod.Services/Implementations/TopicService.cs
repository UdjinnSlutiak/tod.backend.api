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
using Tod.Services.Requests;
using Tod.Services.Responses;

namespace Tod.Services.Implementations
{
	public class TopicService : ITopicService
	{
        private readonly ITopicRepository topicRepository;
        private readonly IRepository<TopicTag> topicTagRepository;
        private readonly IUserTopicRepository userTopicRepository;
        private readonly IFavoriteRepository favoriteRepository;
        private readonly IUserService userService;
        private readonly ITagService tagService;
        private readonly IReactionService reactionService;

        public TopicService(ITopicRepository topicRepository,
            IRepository<TopicTag> topicTagRepository,
            IUserTopicRepository userTopicRepository,
            IFavoriteRepository favoriteRepository,
            IUserService userService,
            ITagService tagService,
            IReactionService reactionService)
		{
            this.topicRepository = topicRepository;
            this.topicTagRepository = topicTagRepository;
            this.userTopicRepository = userTopicRepository;
            this.favoriteRepository = favoriteRepository;
            this.userService = userService;
            this.tagService = tagService;
            this.reactionService = reactionService;
		}

        public async Task<Topic> GetByIdAsync(int topicId)
        {
            return await this.topicRepository.GetAsync(topicId);
        }

        public async Task<TopicData> GetTopicDataByIdAsync(int id)
        {
            var topic = await this.topicRepository.GetAsync(id);

            if (topic == null)
            {
                throw new NotFoundException("Topic");
            }

            if (topic.Status == ContentStatus.Banned)
            {
                throw new BannedContentException("topic");
            }

            var authorId = await this.userTopicRepository.GetUserIdByTopicIdAsync(id);

            var user = await this.userService.GetByIdAsync(authorId);

            if (user.Status == ContentStatus.Banned)
            {
                throw new BannedContentException("author");
            }

            var tags = (await this.tagService.GetByTopicIdAsync(id)).ToList();

            var reactions = await this.reactionService.GetByTopicIdAsync(id);

            var positive = reactions.Where(r => r.ReactionValue == ReactionValue.Positive).Count();
            var negative = reactions.Count - positive;

            return new TopicData
            {
                Id = topic.Id,
                Title = topic.Title,
                CreatedUtc = topic.CreatedUtc,
                Author = new UserDto(user),
                Tags = tags,
                Rating = positive - negative
            };
        }

        public async Task<GetTopicsResponse> GetTopicsAsync(int skip, int offset)
        {
            var topics = await this.topicRepository.GetTopicsRangeAsync(skip, offset);

            if (topics == null)
            {
                return new GetTopicsResponse();
            }

            var topicsData = new List<TopicData>();
            foreach (var topic in topics)
            {
                var authorId = await this.userTopicRepository.GetUserIdByTopicIdAsync(topic.Id);

                var author = await this.userService.GetByIdAsync(authorId);

                var tags = (await this.tagService.GetByTopicIdAsync(topic.Id)).ToList();

                var reactions = await this.reactionService.GetByTopicIdAsync(topic.Id);

                var positive = reactions.Where(r => r.ReactionValue == ReactionValue.Positive).Count();
                var negative = reactions.Count - positive;

                topicsData.Add(new TopicData
                {
                    Id = topic.Id,
                    Title = topic.Title,
                    CreatedUtc = topic.CreatedUtc,
                    Author = new UserDto(author),
                    Tags = tags,
                    Rating = positive - negative
                });
            }

            return new GetTopicsResponse
            {
                Topics = topicsData
            };
        }

        public async Task<CreateTopicResponse> CreateAsync(CreateTopicRequest request, int userId)
        {
            var user = await this.userService.GetByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("User");
            }

            var topicSearchResult = await this.topicRepository.GetByTitleAsync(request.Title);

            if (topicSearchResult != null)
            {
                throw new TopicAlreadyExistsException();
            }

            var topic = new Topic
            {
                Title = request.Title,
                CreatedUtc = DateTime.UtcNow,
                Status = ContentStatus.Ok
            };

            topic = await this.topicRepository.CreateAsync(topic);

            await this.CreateUserTopicAsync(userId, topic.Id);

            var topicTags = new List<Tag>();
            foreach (var title in request.Tags)
            {
                var tag = await this.tagService.GetByTitleAsync(title);

                if (tag != null && tag.Status == ContentStatus.Banned)
                {
                    continue;
                }

                if (tag == null)
                {
                    tag = await this.tagService.CreateAsync(title, userId);
                }

                await this.tagService.IncreaseUsedCountAsync(tag);

                await this.CreateTopicTagAsync(tag.Id, topic.Id);

                topicTags.Add(tag);
            }

            return new CreateTopicResponse
            {
                Id = topic.Id,
                Title = topic.Title,
                Author = new UserDto(user),
                CreatedUtc = topic.CreatedUtc,
                Tags = topicTags
            };
        }

        public async Task<bool> AddToFavoritesAsync(int topicId, int userId)
        {
            var topic = await this.topicRepository.GetAsync(topicId);

            if (topic == null)
            {
                throw new NotFoundException("Topic");
            }

            if (topic.Status == ContentStatus.Banned)
            {
                throw new BannedContentException("topic");
            }

            var user = await this.userService.GetByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("User");
            }

            if (user.Status == ContentStatus.Banned)
            {
                throw new BannedContentException("user");
            }

            var authorId = await this.userTopicRepository.GetUserIdByTopicIdAsync(topicId);

            if (userId == authorId)
            {
                throw new ContentBelongsToYouException();
            }

            var fromFavorites = await this.favoriteRepository.GetByUserIdAndTopicId(userId, topicId);

            if (fromFavorites != null)
            {
                throw new TopicAlreadyInFavoritesException();
            }

            var favorite = new FavoriteTopic
            {
                UserId = userId,
                TopicId = topicId
            };

            await this.favoriteRepository.CreateAsync(favorite);

            await this.UpdateAuthorRatingAsync(authorId, 1);

            return true;
        }

        private async Task UpdateAuthorRatingAsync(int authorId, int value)
        {
            var author = await this.userService.GetByIdAsync(authorId);
            author.Rating += 1;
            await this.userService.UpdateAsync(author);
        }

        private async Task CreateTopicTagAsync(int tagId, int topicId)
        {
            var topicTag = new TopicTag
            {
                TagId = tagId,
                TopicId = topicId
            };

            await this.topicTagRepository.CreateAsync(topicTag);
        }

        private async Task CreateUserTopicAsync(int userId, int topicId)
        {
            var userTopic = new UserTopic
            {
                UserId = userId,
                TopicId = topicId
            };

            await this.userTopicRepository.CreateAsync(userTopic);
        }
    }
}

