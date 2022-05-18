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
        private readonly ITopicTagRepository topicTagRepository;
        private readonly IUserTopicRepository userTopicRepository;
        private readonly IFavoriteRepository favoriteRepository;
        private readonly IUserService userService;
        private readonly ITagService tagService;
        private readonly IReactionService reactionService;

        public TopicService(ITopicRepository topicRepository,
            ITopicTagRepository topicTagRepository,
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

        public async Task<TopicData> GetTopicDataByIdAsync(int userId, int id)
        {
            var topic = await this.topicRepository.GetAsync(id);

            if (topic == null)
            {
                throw new NotFoundException(ContentType.Topic);
            }

            if (topic.Status == ContentStatus.Banned)
            {
                throw new BannedContentException(ContentType.Topic);
            }

            var authorId = await this.userTopicRepository.GetUserIdByTopicIdAsync(id);

            var user = await this.userService.GetByIdAsync(authorId);

            if (user.Status == ContentStatus.Banned)
            {
                throw new BannedContentException(ContentType.User);
            }

            var tags = (await this.tagService.GetByTopicIdAsync(id)).ToList();

            var reactions = await this.reactionService.GetByTopicIdAsync(id);

            var positive = reactions.Where(r => r.ReactionValue == ReactionValue.Positive).Count();
            var negative = reactions.Count - positive;

            var fromFavorite = (FavoriteTopic)null;
            if (userId != 0)
            {
                fromFavorite = await this.favoriteRepository.GetByUserIdAndTopicId(userId, topic.Id);
            }

            return new TopicData
            {
                Id = topic.Id,
                Title = topic.Title,
                CreatedUtc = topic.CreatedUtc,
                Author = new UserDto(user),
                Tags = tags,
                Rating = positive - negative,
                IsInFavorite = fromFavorite != null
            };
        }

        public async Task<GetTopicsResponse> GetTopicsAsync(int userId, int skip, int offset)
        {
            var topics = await this.topicRepository.GetRangeAsync(skip, offset);

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

                var fromFavorite = (FavoriteTopic)null;
                if (userId != 0)
                {
                    fromFavorite = await this.favoriteRepository.GetByUserIdAndTopicId(userId, topic.Id);
                }

                topicsData.Add(new TopicData
                {
                    Id = topic.Id,
                    Title = topic.Title,
                    CreatedUtc = topic.CreatedUtc,
                    Author = new UserDto(author),
                    Tags = tags,
                    Rating = positive - negative,
                    IsInFavorite = fromFavorite != null
                });
            }

            return new GetTopicsResponse
            {
                Topics = topicsData
            };
        }

        public async Task<GetTopicsResponse> GetFavoritesAsync(int userId)
        {
            var user = await this.userService.GetByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException(ContentType.User);
            }

            var userFavoritesIds = await this.favoriteRepository.GetByUserId(userId);

            var topicsData = await this.GetTopicsDataByTopicsIds(userFavoritesIds);

            return new GetTopicsResponse
            {
                Topics = topicsData
            };
        }

        public async Task<GetTopicsResponse> GetMyTopicsAsync(int userId)
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

            var userTopicsIds = await this.userTopicRepository.GetTopicsIdByUserId(userId);

            var topics = await this.GetTopicsDataByTopicsIds(userTopicsIds);

            return new GetTopicsResponse
            {
                Topics = topics
            };
        }

        public async Task<CreateTopicResponse> CreateAsync(CreateTopicRequest request, int userId)
        {
            var user = await this.userService.GetByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException(ContentType.User);
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
                throw new NotFoundException(ContentType.Topic);
            }

            if (topic.Status == ContentStatus.Banned)
            {
                throw new BannedContentException(ContentType.Topic);
            }

            var user = await this.userService.GetByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException(ContentType.User);
            }

            if (user.Status == ContentStatus.Banned)
            {
                throw new BannedContentException(ContentType.User);
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

        public async Task<GetTopicsResponse> SearchTopicsAsync(TopicSearchRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Author)
                && string.IsNullOrWhiteSpace(request.Title)
                && (request.Tags == null || request.Tags.Count == 0))
            {
                return null;
            }

            var topicsIds = (List<int>)null;

            if (!string.IsNullOrWhiteSpace(request.Author))
            {
                topicsIds = new();
                topicsIds.AddRange(await this.SearchTopicByAuthorAsync(request.Author));
            }

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                topicsIds = await this.SearchTopicByTitleAsync(request.Title, topicsIds);
            }

            if (request.Tags == null || request.Tags.Count != 0)
            {
                topicsIds = await this.SearchTopicByTagsAsync(request.Tags, topicsIds);
            }

            if (topicsIds == null || topicsIds.Count == 0)
            {
                throw new NotFoundException(ContentType.Topic);
            }

            var topicsData = await this.GetTopicsDataByTopicsIds(topicsIds);

            return new GetTopicsResponse
            {
                Topics = topicsData
            };
        }

        private async Task<List<TopicData>> GetTopicsDataByTopicsIds(List<int> topicsIds)
        {
            var topicsData = new List<TopicData>();

            foreach (var topicId in topicsIds)
            {
                var topic = await this.topicRepository.GetAsync(topicId);

                if (topic == null || topic.Status == ContentStatus.Banned)
                {
                    continue;
                }

                var authorId = await this.userTopicRepository.GetUserIdByTopicIdAsync(topicId);

                var author = await this.userService.GetByIdAsync(authorId);

                if (author == null || author.Status == ContentStatus.Banned)
                {
                    continue;
                }

                var tags = (await this.tagService.GetByTopicIdAsync(topicId)).ToList();

                var reactions = await this.reactionService.GetByTopicIdAsync(topicId);

                var positive = reactions.Where(r => r.ReactionValue == ReactionValue.Positive).Count();
                var negative = reactions.Count - positive;

                topicsData.Add(new TopicData
                {
                    Id = topicId,
                    Title = topic.Title,
                    CreatedUtc = topic.CreatedUtc,
                    Author = new UserDto(author),
                    Tags = tags,
                    Rating = positive - negative
                });
            }

            return topicsData;
        }

        private async Task<List<int>> SearchTopicByAuthorAsync(string username)
        {
            var topicsByAuthor = await this.GetTopicsIdByAuthorAsync(username);

            if (topicsByAuthor == null || topicsByAuthor.Count == 0)
            {
                throw new NotFoundException(ContentType.Topic);
            }

            return topicsByAuthor;
        }

        private async Task<List<int>> SearchTopicByTitleAsync(string title, List<int> topicsIds)
        {
            var topicsByTitle = await this.GetTopicsIdByTitle(title);

            if (topicsByTitle == null || topicsByTitle.Count == 0)
            {
                throw new NotFoundException(ContentType.Topic);
            }

            if (topicsIds == null)
            {
                topicsIds = new();
                topicsIds.AddRange(topicsByTitle);

                return topicsIds;
            }

            var filtered = topicsIds.Intersect(topicsByTitle).ToList();

            if (filtered.Count == 0)
            {
                throw new NotFoundException(ContentType.Topic);
            }

            return filtered;
        }

        private async Task<List<int>> SearchTopicByTagsAsync(List<string> tags, List<int> topicsIds)
        {
            var topicsByTags = await this.GetTopicsIdByTags(tags);

            if (topicsByTags == null || topicsByTags.Count == 0)
            {
                throw new NotFoundException(ContentType.Topic);
            }

            if (topicsIds == null)
            {
                topicsIds = new();
                topicsIds.AddRange(topicsByTags);

                return topicsIds;
            }

            var filtered = topicsIds.Intersect(topicsByTags).ToList();

            if (filtered.Count == 0)
            {
                throw new NotFoundException(ContentType.Topic);
            }

            return filtered;
        }

        private async Task<List<int>> GetTopicsIdByAuthorAsync(string username)
        {
            var user = await this.userService.GetByUsernameAsync(username);

            if (user == null)
            {
                throw new NotFoundException(ContentType.User);
            }

            return await this.userTopicRepository.GetTopicsIdByUserId(user.Id);
        }

        private async Task<List<int>> GetTopicsIdByTitle(string title)
        {
            return (await this.topicRepository.GetWhereTitleContainsAsync(title)).Select(t => t.Id).ToList();
        }

        private async Task<List<int>> GetTopicsIdByTags(List<string> tagsTitles)
        {
            var topicsIds = (List<int>)null;

            foreach (var tagTitle in tagsTitles)
            {
                var tag = await this.tagService.GetByTitleAsync(tagTitle);

                if (tag == null)
                    return null;

                var topicsIdsByTag = await this.topicTagRepository.GetByTagIdAsync(tag.Id);

                if (topicsIds == null)
                {
                    topicsIds = new();
                    topicsIds.AddRange(topicsIdsByTag);

                    if (topicsIds.Count == 0)
                    {
                        return null;
                    }

                    continue;
                }

                var filtered = topicsIdsByTag.Intersect(topicsIds).ToList();

                if (filtered.Count == 0)
                {
                    return null;
                }

                topicsIds = filtered;
            }

            return topicsIds;
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

