using System;
using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Domain.Repositories.Abstractions;
using Tod.Services.Abstractions;

namespace Tod.Services.Implementations
{
	public class UserService : IUserService
	{
        private readonly IUserRepository userRepository;

		public UserService(IUserRepository userRepository)
		{
            this.userRepository = userRepository;
		}

        public async Task<User> GetByIdAsync(int id)
        {
            return await this.userRepository.GetAsync(id);
        }

        public async Task<User> UpdateAsync(User user)
        {
            return await this.userRepository.UpdateAsync(user);
        }
    }
}

