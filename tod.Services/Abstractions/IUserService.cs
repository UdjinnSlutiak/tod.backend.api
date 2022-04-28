using System;
using System.Threading.Tasks;
using Tod.Domain.Models;

namespace Tod.Services.Abstractions
{
	public interface IUserService
	{
		public Task<User> GetUserByIdAsync(int id);
	}
}

