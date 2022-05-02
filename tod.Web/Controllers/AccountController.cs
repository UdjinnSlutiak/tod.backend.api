using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tod.Domain.Models;
using Tod.Services.Abstractions;
using Tod.Services.Exceptions;
using Tod.Services.Requests;
using Tod.Services.Responses;

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
			catch (EmailAlreadyTakenException)
            {
				return BadRequest("User with such email already exists.");
            }
			catch (UsernameAlreadyTakenException)
            {
				return BadRequest("User with such username already exists.");
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
			catch (NotFoundException)
            {
				return Unauthorized("User with such email does not registered in the system.");
            }
			catch (PasswordMismatchException)
            {
				return Unauthorized("Wrong password.");
            }
        }
	}
}

