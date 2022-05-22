using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tod.Domain.Models;
using Tod.Domain.Models.Dtos;
using Tod.Services.Abstractions;
using Tod.Services.Exceptions;
using Tod.Services.Requests;
using Tod.Services.Responses;
using Tod.Web.Extensions;

namespace Tod.Web.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/account")]
	public class AccountController : ControllerBase
	{
		private readonly IAccountService accountService;
		private readonly IInterestTagService interestTagService;

		public AccountController(IAccountService accountService,
			IInterestTagService interestTagService)
		{
			this.accountService = accountService;
			this.interestTagService = interestTagService;
		}

		[AllowAnonymous]
		[HttpPost("register")]
		[ProducesResponseType(typeof(LoginResponse), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<LoginResponse>> Register(RegisterRequest request)
        {
			User user;

            try
            {
				user = await this.accountService.RegisterUserAsync(request);
            }
			catch (EmailAlreadyTakenException ex)
            {
				return BadRequest(ex.Message);
            }
			catch (UsernameAlreadyTakenException ex)
            {
				return BadRequest(ex.Message);
            }

			var response = await this.accountService.LoginUserAsync(user);

			return response;
        }

		[AllowAnonymous]
		[HttpPost("login")]
		[ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
			try
            {
				var response = await this.accountService.LoginUserAsync(request);

				return response;
            }
			catch (NotFoundException ex)
            {
				return Unauthorized(ex.Message);
            }
			catch (PasswordMismatchException ex)
            {
				return Unauthorized(ex.Message);
            }
        }

		[HttpPost("refresh-token")]
		[ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<LoginResponse>> RefreshToken(RefreshTokenRequest request)
        {
			try
            {
				var response = await this.accountService.RefreshTokenAsync(request);

				return response;
            }
			catch (InvalidTokenException ex)
            {
				return BadRequest(ex.Message);
            }
			catch (RedisException ex)
            {
				return Forbid(ex.Message);
            }
        }

		[HttpGet("me")]
		[ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<UserDto>> GetMe()
        {
			try
            {
				var currentUserId = base.HttpContext.GetCurrentUserId();

				var user = await this.accountService.GetUserByIdAsync(currentUserId);

				return user;
            }
			catch (InvalidTokenException ex)
			{
				return Unauthorized(ex.Message);
			}
			catch (NotFoundException ex)
            {
				return Unauthorized( ex.Message);
            }
        }

		[HttpGet("interests")]
		[ProducesResponseType(typeof(InterestTagsResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<InterestTagsResponse>> GetUserInterestTags()
        {
			try
            {
				var currentUserId = base.HttpContext.GetCurrentUserId();
				var tags = await this.interestTagService.GetUserInterestTagsAsync(currentUserId);

				return tags;
			}
			catch (InvalidTokenException ex)
			{
				return Unauthorized(ex.Message);
			}
			catch (NotFoundException ex)
			{
				return Unauthorized(ex.Message);
			}
		}

		[HttpPut("interests")]
		[ProducesResponseType(typeof(InterestTagsResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<InterestTagsResponse>> UpdateUserTagInterests(UpdateInterestTagsRequest request)
		{
			try
			{
				var currentUserId = base.HttpContext.GetCurrentUserId();
				var tags = await this.interestTagService.UpdateUserInterestTagsAsync(currentUserId, request.TagsText);

				return tags;
			}
			catch (InvalidTokenException ex)
			{
				return Unauthorized(ex.Message);
			}
			catch (NotFoundException ex)
			{
				return Unauthorized(ex.Message);
			}
		}
	}
}

