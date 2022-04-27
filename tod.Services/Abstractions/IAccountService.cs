using System;
using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Domain.Models.Dtos;
using Tod.Services.Requests;
using Tod.Services.Responses;

namespace Tod.Services.Abstractions
{
	public interface IAccountService
	{
		public Task<UserDto> GetUserByEmailAsync(string email);
		public Task<UserDto> GetUserByUsernameAsync(string username);
		public Task<UserDto> RegisterUserAsync(RegisterRequest request);
		public Task<LoginResponse> LoginUserAsync(string login, string password);
		public Task<LoginResponse> LoginUserAsync(User user);
	}
}

