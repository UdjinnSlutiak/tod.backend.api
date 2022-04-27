using System;
using Tod.Domain.Models;

namespace Tod.Services.Abstractions
{
	public interface ITokenService
	{
		public string GetAccessToken(User user);
		public string GetRefreshToken();
		public bool ValidateAccessToken(string token);
	}
}

