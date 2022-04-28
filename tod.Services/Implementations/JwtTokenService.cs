using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Tod.Domain.Models;
using Tod.Services.Abstractions;
using Tod.Services.Exceptions;
using Tod.Services.Jwt;

namespace Tod.Services.Implementations
{
	public class JwtTokenService : ITokenService
	{
        private readonly JwtTokenConfiguration jwtConfig;
        private readonly IRedisService redisService;
        private readonly IUserService userService;

		public JwtTokenService(
            JwtTokenConfiguration jwtConfig,
            IRedisService redisService,
            IUserService userService)
		{
            this.jwtConfig = jwtConfig;
            this.redisService = redisService;
            this.userService = userService;
		}

        public string GetAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(this.jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim("UserId", user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    }),
                Expires = DateTime.UtcNow.AddMinutes(this.jwtConfig.AccessTokenExpiresInMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature),
                Issuer = this.jwtConfig.Issuer,
                Audience = this.jwtConfig.Audience,
                NotBefore = DateTime.UtcNow
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GetRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        public bool ValidateAccessToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(this.jwtConfig.Secret);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return validatedToken != null;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User> ValidateTokenRefreshing(string accessToken, string refreshToken)
        {
            var claimsPrincipal = this.GetPrincipalFromToken(accessToken);

            if (GetPrincipalFromToken == null)
            {
                throw new InvalidTokenException();
            }

            var userId = int.Parse(claimsPrincipal.Claims.First(c => c.Type == "UserId").Value);
            var user = await this.userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new InvalidTokenException();
            }

            var userPasswordHashCode = user.PasswordHash.GetHashCode();
            var refreshTokenSearchResult = await this.redisService.GetRefreshTokenAsync(accessToken, userPasswordHashCode);

            if (refreshTokenSearchResult == null || refreshToken != refreshTokenSearchResult)
            {
                throw new InvalidTokenException();
            }

            return user;
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.jwtConfig.Secret));
            var creds = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha256);

            var parameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = tokenKey,
                ValidateLifetime = false
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, parameters, out var securityToken);

                if (!(securityToken is JwtSecurityToken))
                {
                    throw new InvalidTokenException();
                }

                return principal;
            }
            catch
            {
                throw new UnableToValidateTokenException();
            }
        }
    }
}

