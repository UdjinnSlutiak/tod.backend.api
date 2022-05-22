using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Domain.Models.Enums;
using Tod.Domain.Repositories.Abstractions;
using Tod.Services.Abstractions;
using Tod.Services.Exceptions;
using Tod.Services.Responses;

namespace Tod.Services.Implementations
{
	public class InterestTagService : IInterestTagService
	{
		private readonly IInterestTagRepository interestTagRepository;
		private readonly ITagService tagService;
        private readonly IContentValidator contentValidator;

		public InterestTagService(IInterestTagRepository interestTagRepository,
			ITagService tagService,
            IContentValidator contentValidator)
		{
			this.interestTagRepository = interestTagRepository;
			this.tagService = tagService;
            this.contentValidator = contentValidator;
		}

        public async Task<InterestTagsResponse> GetUserInterestTagsAsync(int userId)
        {
            var user = await this.contentValidator.GetAndValidateUserAsync(userId);
            var tagsIds = (await this.interestTagRepository.GetInterestTagsByUserIdAsync(userId)).Select(it => it.TagId);

            var tags = new List<Tag>();
            foreach (var tagId in tagsIds)
            {
                var tag = await this.tagService.GetByIdAsync(tagId);
                if (tag == null)
                {
                    continue;
                }
                tags.Add(tag);
            }

            return new InterestTagsResponse
            {
                Tags = tags
            };
        }

        public async Task<InterestTagsResponse> UpdateUserInterestTagsAsync(int userId, List<string> tagsText)
        {
            var user = await this.contentValidator.GetAndValidateUserAsync(userId);

            await this.interestTagRepository.DeleteUserInterestTagsAsync(userId);

            var tags = new List<Tag>();
            foreach (var tagText in tagsText)
            {
                var tag = await this.tagService.GetByTitleAsync(tagText);

                if (tag == null)
                {
                    tag = await this.tagService.CreateAsync(tagText, userId);
                }

                if (tag.Status == ContentStatus.Banned)
                {
                    continue;
                }

                tags.Add(tag);

                var interestTag = new InterestTag
                {
                    UserId = userId,
                    TagId = tag.Id
                };
                await this.interestTagRepository.CreateAsync(interestTag);
            }

            return new InterestTagsResponse
            {
                Tags = tags
            };
        }
    }
}

