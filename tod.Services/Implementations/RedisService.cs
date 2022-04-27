using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Tod.Services.Abstractions;
using Tod.Services.Exceptions;

namespace Tod.Services.Implementations
{
	public class RedisService : IRedisService
	{
        private readonly IDistributedCache distributedCache;

		public RedisService(IDistributedCache distributedCache)
		{
            this.distributedCache = distributedCache;
		}

        public async Task<string> GetRefreshTokenAsync(string accessToken, int passwordHashCode, CancellationToken cancellationToken = default)
        {
            var key = this.GetKey(accessToken, passwordHashCode);

            try
            {
                var refreshToken = await this.distributedCache.GetStringAsync(key, cancellationToken);

                return refreshToken;
            }
            catch
            {
                throw new RedisException();
            }
        }

        public async Task<bool> CheckRefreshTokenExistanceAsync(string accessToken, int passwordHashCode, CancellationToken cancellationToken = default)
        {
            var key = this.GetKey(accessToken, passwordHashCode);

            try
            {
                var refreshToken = await this.GetRefreshTokenAsync(accessToken, passwordHashCode, cancellationToken);

                return refreshToken != null;
            }
            catch
            {
                throw new RedisException();
            }
        }

        public async Task SaveAsync(string accessToken, int passwordHashCode, string refreshToken, TimeSpan deleteAfter, CancellationToken cancellationToken = default)
        {
            try
            {
                var key = this.GetKey(accessToken, passwordHashCode);
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = deleteAfter
                };

                await this.distributedCache.SetStringAsync(key, refreshToken, options, cancellationToken);
            }
            catch
            {
                throw new RedisException();
            }
        }

        public async Task DeleteAsync(string accessToken, int passwordHashCode, CancellationToken cancellationToken = default)
        {
            try
            {
                var key = this.GetKey(accessToken, passwordHashCode);

                await this.distributedCache.RemoveAsync(key, cancellationToken);
            }
            catch
            {
                throw new RedisException();
            }
        }

        private string GetKey(string accessToken, int passwordHashCode)
        {
            return $"{accessToken}:{passwordHashCode}";
        }
    }
}

