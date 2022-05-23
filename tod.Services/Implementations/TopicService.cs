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
        private readonly ICommentaryService commentaryService;
        private readonly ITagService tagService;
        private readonly IReactionService reactionService;
        private readonly IInterestTagService interestTagService;
        private readonly IContentValidator contentValidator;

        public TopicService(ITopicRepository topicRepository,
            ITopicTagRepository topicTagRepository,
            IUserTopicRepository userTopicRepository,
            IFavoriteRepository favoriteRepository,
            IUserService userService,
            ICommentaryService commentaryService,
            ITagService tagService,
            IReactionService reactionService,
            IInterestTagService interestTagService,
            IContentValidator contentValidator)
		{
            this.topicRepository = topicRepository;
            this.topicTagRepository = topicTagRepository;
            this.userTopicRepository = userTopicRepository;
            this.favoriteRepository = favoriteRepository;
            this.userService = userService;
            this.commentaryService = commentaryService;
            this.tagService = tagService;
            this.reactionService = reactionService;
            this.interestTagService = interestTagService;
            this.contentValidator = contentValidator;
		}

        public async Task<Topic> GetByIdAsync(int topicId)
        {
            return await this.topicRepository.GetAsync(topicId);
        }

        public async Task<TopicData> GetTopicByIdAsync(int userId, int id)
        {
            var topic = await this.contentValidator.GetAndValidateTopicAsync(id);

            var authorId = await this.userTopicRepository.GetUserIdByTopicIdAsync(id);

            var user = await this.contentValidator.GetAndValidateUserAsync(authorId);

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

        public async Task<GetTopicsResponse> GetTopicsAsync(int userId, int skip = 0, int offset = 20)
        {
            var topics = await this.topicRepository.GetRangeAsync(skip, offset);

            if (topics == null)
            {
                return new GetTopicsResponse
                {
                    Topics = new()
                };
            }

            var topicsData = new List<TopicData>();
            foreach (var topic in topics)
            {
                var authorId = await this.userTopicRepository.GetUserIdByTopicIdAsync(topic.Id);

                var author = await this.userService.GetByIdAsync(authorId);

                var tags = (await this.tagService.GetByTopicIdAsync(topic.Id)).ToList();

                var rating = await this.GetTopicRating(topic.Id);

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
                    Rating = rating,
                    IsInFavorite = fromFavorite != null
                });
            }

            return new GetTopicsResponse
            {
                Topics = topicsData.Skip(skip).Take(offset).ToList()
            };
        }

        public async Task<GetTopicsResponse> GetFavoritesAsync(int userId, int skip = 0, int offset = 20)
        {
            var user = await this.contentValidator.GetAndValidateUserAsync(userId);

            var userFavoritesIds = this.favoriteRepository.GetByUserId(userId);

            if (userFavoritesIds.Count == 0)
            {
                return new GetTopicsResponse
                {
                    Topics = new()
                };
            }

            var topicsData = await this.GetTopicsDataByTopicsIds(userFavoritesIds);

            return new GetTopicsResponse
            {
                Topics = topicsData.Skip(skip).Take(offset).ToList()
            };
        }

        public async Task<GetTopicsResponse> GetMyTopicsAsync(int userId, int skip = 0, int offset = 20)
        {
            var user = await this.contentValidator.GetAndValidateUserAsync(userId);

            var userTopicsIds = this.userTopicRepository.GetTopicsIdByUserId(userId);

            if (userTopicsIds.Count == 0)
            {
                return new GetTopicsResponse
                {
                    Topics = new()
                };
            }

            var topics = await this.GetTopicsDataByTopicsIds(userTopicsIds);

            return new GetTopicsResponse
            {
                Topics = topics.Skip(skip).Take(offset).ToList()
            };
        }

        public async Task<GetTopicsResponse> GetDiscussedTopicsAsync(int userId, int skip = 0, int offset = 20)
        {
            var user = await this.contentValidator.GetAndValidateUserAsync(userId);

            var discussedTopicsIds = await this.commentaryService.GetLatestDiscussedTopicsIds(userId);
            var discussedTopics = new List<TopicData>();
            foreach (var topicId in discussedTopicsIds)
            {
                var topicData = await this.GetTopicByIdAsync(userId, topicId);
                if (topicData == null)
                {
                    continue;
                }
                discussedTopics.Add(topicData);
            }

            return new GetTopicsResponse
            {
                Topics = discussedTopics.Skip(skip).Take(offset).ToList()
            };
        }

        public async Task<GetTopicsResponse> GetRecommendedTopicsAsync(int userId, int skip = 0, int offset = 20)
        {
            var user = await this.contentValidator.GetAndValidateUserAsync(userId);

            var interestTags = await this.interestTagService.GetUserInterestTagsAsync(userId);
            var topicsIds = new List<int>();
            foreach (var tag in interestTags.Tags)
            {
                var topicsIdsByTag = await this.topicTagRepository.GetByTagIdAsync(tag.Id);
                topicsIds = topicsIds.Union(topicsIdsByTag).ToList();
            }

            var topics = (await this.GetTopicsDataByTopicsIds(topicsIds.OrderByDescending(t => t).ToList()))
                .Skip(skip).Take(offset).ToList();

            return new GetTopicsResponse
            {
                Topics = topics
            };
        }

        public async Task<CreateTopicResponse> CreateAsync(CreateTopicRequest request, int userId)
        {
            var user = await this.contentValidator.GetAndValidateUserAsync(userId);

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

            var topicTags = await this.GetTopicTagsAsync(userId, topic.Id, request.Tags);

            return new CreateTopicResponse
            {
                Id = topic.Id,
                Title = topic.Title,
                Author = new UserDto(user),
                CreatedUtc = topic.CreatedUtc,
                Tags = topicTags
            };
        }

        public async Task<TopicData> UpdateTopicAsync(int userId, int topicId, CreateTopicRequest request)
        {
            var user = await this.contentValidator.GetAndValidateUserAsync(userId);
            var authorId = await this.userTopicRepository.GetUserIdByTopicIdAsync(topicId);
            if (userId != authorId)
            {
                throw new PermissionDeniedException();
            }

            var topic = await this.contentValidator.GetAndValidateTopicAsync(topicId);

            var topicByTitle = await this.topicRepository.GetByTitleAsync(request.Title);
            if (topicByTitle != null)
            {
                throw new TopicAlreadyExistsException();
            }

            topic.Title = request.Title;
            topic = await this.topicRepository.UpdateAsync(topic);
            await this.topicTagRepository.DeleteTopicTagsAsync(topicId);
            var topicTags = await this.GetTopicTagsAsync(userId, topicId, request.Tags);

            var rating = await this.GetTopicRating(topicId);

            return new TopicData
            {
                Id = topic.Id,
                Title = topic.Title,
                CreatedUtc = topic.CreatedUtc,
                Author = new UserDto(user),
                Tags = topicTags,
                Rating = rating,
                IsInFavorite = false,
            };
        }

        public async Task<bool> AddToFavoritesAsync(int topicId, int userId)
        {
            var topic = await this.contentValidator.GetAndValidateTopicAsync(topicId);

            var user = await this.contentValidator.GetAndValidateUserAsync(userId);

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

        public async Task MarkTopicDeletedAsync(int userId, int topicId)
        {
            var user = await this.contentValidator.GetAndValidateUserAsync(userId);
            var authorId = await this.userTopicRepository.GetUserIdByTopicIdAsync(topicId);
            if (userId != authorId)
            {
                throw new PermissionDeniedException();
            }

            var topic = await this.contentValidator.GetAndValidateTopicAsync(topicId);
            topic.Status = ContentStatus.DeletedByOwner;
            await this.topicRepository.UpdateAsync(topic);
        }

        private async Task<List<TopicData>> GetTopicsDataByTopicsIds(List<int> topicsIds)
        {
            var topicsData = new List<TopicData>();

            foreach (var topicId in topicsIds)
            {
                var topic = await this.topicRepository.GetAsync(topicId);

                if (topic == null || topic.Status == ContentStatus.Banned || topic.Status == ContentStatus.DeletedByOwner)
                {
                    continue;
                }

                var authorId = await this.userTopicRepository.GetUserIdByTopicIdAsync(topicId);

                var author = await this.userService.GetByIdAsync(authorId);

                if (author == null || author.Status == ContentStatus.Banned || author.Status == ContentStatus.DeletedByOwner)
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

        private async Task<List<Tag>> GetTopicTagsAsync(int userId, int topicId, List<string> tagsTitles)
        {
            var topicTags = new List<Tag>();
            foreach (var title in tagsTitles)
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

                await this.CreateTopicTagAsync(tag.Id, topicId);

                topicTags.Add(tag);
            }

            return topicTags;
        }

        private async Task<int> GetTopicRating(int topicId)
        {
            var reactions = await this.reactionService.GetByTopicIdAsync(topicId);

            var positive = reactions.Where(r => r.ReactionValue == ReactionValue.Positive).Count();
            var negative = reactions.Count - positive;

            return positive - negative;
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

            return this.userTopicRepository.GetTopicsIdByUserId(user.Id);
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

