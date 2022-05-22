using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Domain.Models.Enums;
using Tod.Domain.Repositories.Abstractions;
using Tod.Services.Abstractions;

namespace Tod.Services.Implementations
{
	public class TagService : ITagService
	{
        private readonly ITagRepository tagRepository;
        private readonly ITopicTagRepository topicTagRepository;

		public TagService(ITagRepository tagRepository,
            ITopicTagRepository topicTagRepository)
		{
            this.tagRepository = tagRepository;
            this.topicTagRepository = topicTagRepository;
		}

        public async Task<Tag> GetByIdAsync(int tagId)
        {
            var tag = await this.tagRepository.GetAsync(tagId);

            return this.Validate(tag);
        }

        public async Task<Tag> GetByTitleAsync(string title)
        {
            return await this.tagRepository.GetByTitleAsync(title);
        }

        public async Task<List<Tag>> GetByTopicIdAsync(int topicId)
        {
            var tagIds = await this.topicTagRepository.GetByTopicIdAsync(topicId);

            var tags = new List<Tag>();
            foreach (var tagId in tagIds)
            {
                var tag = await this.tagRepository.GetAsync(tagId);

                if (tag.Status != ContentStatus.Banned)
                {
                    tags.Add(tag);
                }
            }

            return tags;
        }

        public async Task<Tag> CreateAsync(string text, int userId)
        {
            var tag = new Tag
            {
                Text = text,
                Status = ContentStatus.Ok,
                UserId = userId,
                UsedCount = 0
            };

            await this.tagRepository.CreateAsync(tag);

            return tag;
        }

        public async Task<Tag> IncreaseUsedCountAsync(Tag tag)
        {
            tag.UsedCount++;

            await this.tagRepository.UpdateAsync(tag);

            return tag;
        }

        private Tag Validate(Tag tag)
        {
            if (tag == null)
            {
                return tag;
            }

            if (tag.Status == ContentStatus.Banned)
            {
                return null;
            }

            return tag;
        }
    }
}

