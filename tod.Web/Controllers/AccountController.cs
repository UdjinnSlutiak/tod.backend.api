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

		public AccountController(IAccountService accountService)
		{
			this.accountService = accountService;
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
	}
}

