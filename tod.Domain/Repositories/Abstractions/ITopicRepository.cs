using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;

namespace Tod.Domain.Repositories.Abstractions
{
	public interface ITopicRepository : IRepository<Topic>
	{
		public Task<Topic> GetByTitleAsync(string title);
		public Task<List<Topic>> GetWhereTitleContainsAsync(string titlePart);
		public Task<List<Topic>> GetRangeAsync(int skip, int offset);
	}
}

