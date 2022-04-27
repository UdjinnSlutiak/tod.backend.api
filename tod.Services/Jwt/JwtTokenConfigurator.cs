using System;
using Microsoft.Extensions.DependencyInjection;

namespace Tod.Services.Jwt
{
	public static class JwtTokenConfigurator
	{
		public static IServiceCollection AddJwtTokenConfiguration(this IServiceCollection services, out JwtTokenConfiguration jwtConfiguration)
        {
			jwtConfiguration = new JwtTokenConfiguration()
			{
				Secret = Environment.GetEnvironmentVariable("TOD_JWT_SECRET"),
				Issuer = Environment.GetEnvironmentVariable("TOD_JWT_ISSUER"),
				Audience = Environment.GetEnvironmentVariable("TOD_JWT_AUDIENCE"),
				AccessTokenExpiresInMinutes = int.TryParse(Environment.GetEnvironmentVariable("TOD_JWT_AT_EXPIRATION"), out int accExpiration) ? accExpiration : 20,
				RefreshTokenExpiresInMinutes = int.TryParse(Environment.GetEnvironmentVariable("TOD_JWT_RT_EXPIRATION"), out int refExpiration) ? refExpiration : 30,
			};

			services.AddSingleton(jwtConfiguration);

			return services;
        }
	}
}
