using System;
using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Domain.Models.Enums;
using Tod.Domain.Repositories.Abstractions;
using Tod.Services.Abstractions;
using Tod.Services.Exceptions;

namespace Tod.Services.Implementations
{
	public class ContentValidator : IContentValidator
	{
        private readonly IUserService userService;
        private readonly ITopicRepository topicRepository;
        private readonly ICommentaryRepository commentaryRepository;

		public ContentValidator(IUserService userService,
            ITopicRepository topicRepository,
            ICommentaryRepository commentaryRepository)
		{
            this.userService = userService;
            this.topicRepository = topicRepository;
            this.commentaryRepository = commentaryRepository;
		}

        public async Task<User> GetAndValidateUserAsync(int userId)
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

            if (user.Status == ContentStatus.DeletedByOwner)
            {
                throw new DeletedContentException(ContentType.User);
            }

            return user;
        }

        public async Task<Topic> GetAndValidateTopicAsync(int topicId)
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

            if (topic.Status == ContentStatus.DeletedByOwner)
            {
                throw new DeletedContentException(ContentType.Topic);
            }

            return topic;
        }

        public async Task<Commentary> GetAndValidateCommentaryAsync(int commentaryId)
        {
            var commentary = await this.commentaryRepository.GetAsync(commentaryId);

            if (commentary == null)
            {
                throw new NotFoundException(ContentType.Commentary);
            }

            if (commentary.Status == ContentStatus.Banned)
            {
                throw new BannedContentException(ContentType.Commentary);
            }

            if (commentary.Status == ContentStatus.DeletedByOwner)
            {
                throw new DeletedContentException(ContentType.Commentary);
            }

            return commentary;
        }
    }
}

