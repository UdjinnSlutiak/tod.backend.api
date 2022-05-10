using System;
using System.Threading.Tasks;
using Tod.Domain.Models;

namespace Tod.Domain.Repositories.Abstractions
{
	public interface IFavoriteRepository : IRepository<FavoriteTopic>
	{
		public Task<FavoriteTopic> GetByUserIdAndTopicId(int userId, int topicId);
	}
}

