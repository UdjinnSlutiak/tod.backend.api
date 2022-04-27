using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Domain.Models.Dtos;

namespace Tod.Domain.Repositories.Abstractions
{
	public interface IUserRepository : IRepository<User>
	{
		public Task<User> GetUserByEmailAsync(string email);

		public Task<User> GetUserByUsernameAsync(string username);
	}
}

