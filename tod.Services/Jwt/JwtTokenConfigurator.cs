using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Tod.Services.Jwt
{
	public static class JwtTokenConfigurator
	{
		public static IServiceCollection AddJwtTokenConfiguration(this IServiceCollection services, out JwtTokenConfiguration jwtConfiguration)
        {
			using (var reader = new StreamReader("JwtTokenConfig.json"))
            {
				var configJson = reader.ReadToEndAsync().Result;
				var config = JsonConvert.DeserializeObject<JwtTokenConfiguration>(configJson);

				jwtConfiguration = new JwtTokenConfiguration()
				{
					Secret = config.Secret,
					Issuer = config.Issuer,
					Audience = config.Audience,
					AccessTokenExpiresInMinutes = config.AccessTokenExpiresInMinutes,
					RefreshTokenExpiresInMinutes = config.RefreshTokenExpiresInMinutes
				};

				services.AddSingleton(jwtConfiguration);

				return services;
			}
        }
	}
}
