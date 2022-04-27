using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tod.Services.Abstractions
{
	public interface IRedisService
	{
		public Task<string> GetRefreshTokenAsync(string accessToken, int passwordHashCode, CancellationToken cancellationToken = default);
		public Task<bool> CheckRefreshTokenExistanceAsync(string accessToken, int passwordHashCode, CancellationToken cancellationToken = default);
		public Task SaveAsync(string accessToken, int passwordHashCode, string refreshToken, TimeSpan deleteAfter, CancellationToken cancellationToken = default);
		public Task DeleteAsync(string accessToken, int passwordHashCode, CancellationToken cancellationToken = default);
	}
}

