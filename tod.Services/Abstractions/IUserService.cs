using System;
using System.Threading.Tasks;
using Tod.Domain.Models;

namespace Tod.Services.Abstractions
{
	public interface IUserService
	{
		public Task<User> GetByIdAsync(int id);
		public Task<User> GetByUsernameAsync(string username);
		public Task<User> UpdateAsync(User user);
	}
}

