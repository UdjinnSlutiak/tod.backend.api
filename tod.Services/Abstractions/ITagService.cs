using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;

namespace Tod.Services.Abstractions
{
	public interface ITagService
	{
		public Task<Tag> GetByIdAsync(int tagId);
		public Task<Tag> GetByTitleAsync(string title);
		public Task<List<Tag>> GetByTopicIdAsync(int topicId);
		public Task<Tag> CreateAsync(string title, int userId);
		public Task<Tag> IncreaseUsedCountAsync(Tag tag);
	}
}

