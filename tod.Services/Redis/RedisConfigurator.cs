using System;
using Microsoft.Extensions.DependencyInjection;

namespace Tod.Services.Redis
{
	public static class RedisConfigurator
	{
		public static IServiceCollection AddRedisConfiguration(this IServiceCollection services, out RedisConfiguration redisConfiguration)
        {
			redisConfiguration = new RedisConfiguration
			{
				InstanceName = Environment.GetEnvironmentVariable("RedisCacheName"),
				Address = Environment.GetEnvironmentVariable("RedisCacheAddress"),
				Port = int.Parse(Environment.GetEnvironmentVariable("RedisCachePort")),
				Password = Environment.GetEnvironmentVariable("RedisCachePassword"),
			};

			services.AddSingleton(redisConfiguration);

			return services;
        }
	}
}

