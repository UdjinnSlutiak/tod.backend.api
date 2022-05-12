using System;
using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Domain.Models.Dtos;
using Tod.Domain.Models.Enums;
using Tod.Domain.Repositories.Abstractions;
using Tod.Services.Abstractions;
using Tod.Services.Exceptions;
using Tod.Services.Jwt;
using Tod.Services.Requests;
using Tod.Services.Responses;

namespace Tod.Services.Implementations
{
	public class JwtAccountService : IAccountService
	{
		private readonly IUserRepository userRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly ITokenService tokenService;
        private readonly IRedisService redisService;
        private readonly JwtTokenConfiguration jwtConfig;

		public JwtAccountService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IRedisService redisService,
            JwtTokenConfiguration jwtConfig)
		{
			this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.tokenService = tokenService;
            this.redisService = redisService;
            this.jwtConfig = jwtConfig;
		}

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await this.userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                throw new NotFoundException(ContentType.User);
            }

            return new UserDto(user);
        }

        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            var user = await this.userRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                throw new NotFoundException(ContentType.User);
            }

            return new UserDto(user);
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await this.userRepository.GetAsync(id);

            if (user == null)
            {
                throw new NotFoundException(ContentType.User);
            }

            return new UserDto(user);
        }

        public async Task<LoginResponse> LoginUserAsync(LoginRequest request)
        {
            var user = await this.userRepository.GetUserByEmailAsync(request.Login);

            if (user == null)
            {
                user = await this.userRepository.GetUserByUsernameAsync(request.Login);

                if (user == null)
                {
                    throw new NotFoundException(ContentType.User);
                }
            }

            var passwordMatch = this.passwordHasher.VerifyPassword(request.Password, user.PasswordHash);

            if (!passwordMatch)
            {
                throw new PasswordMismatchException();
            }

            return await LoginUserAsync(user);
        }

        public async Task<LoginResponse> LoginUserAsync(User user)
        {
            var accessToken = this.tokenService.GetAccessToken(user);
            var refreshToken = this.tokenService.GetRefreshToken();
            var deleteAfter = TimeSpan.FromMinutes(this.jwtConfig.RefreshTokenExpiresInMinutes);

            await this.redisService.SaveAsync(accessToken, user.PasswordHash.GetHashCode(), refreshToken, deleteAfter);

            return new LoginResponse
            {
                User = new UserDto(user),
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(this.jwtConfig.AccessTokenExpiresInMinutes),
                RefreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(this.jwtConfig.RefreshTokenExpiresInMinutes)
            };
        }

        public async Task<User> RegisterUserAsync(RegisterRequest request)
        {
            var user = await this.userRepository.GetUserByEmailAsync(request.Email);

            if (user != null)
            {
                throw new EmailAlreadyTakenException();
            }

            user = await this.userRepository.GetUserByUsernameAsync(request.Username);

            if (user != null)
            {
                throw new UsernameAlreadyTakenException();
            }

            user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = this.passwordHasher.GetHash(request.Password),
                Rating = 0,
                Role = Role.User,
                Status = ContentStatus.Ok
            };

            await this.userRepository.CreateAsync(user);

            return user;
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var tokensOwner = await this.tokenService.ValidateTokenRefreshing(request.AccessToken, request.RefreshToken);

            await this.redisService.DeleteAsync(request.AccessToken, tokensOwner.PasswordHash.GetHashCode());

            var loginResponse = await this.LoginUserAsync(tokensOwner);

            return loginResponse;
        }
    }
}

