using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Tod.Services.Exceptions;

namespace Tod.Web.Extensions
{
	public static class ContextExtensions
	{
		public static int GetCurrentUserId(this HttpContext context)
        {
			if (context.User == null)
            {
				throw new InvalidTokenException();
            }

			var userIdFromClaims = context.User?.Claims?.FirstOrDefault(c => c.Type == "UserId")?.Value;

			var parsedSuccessfully = int.TryParse(userIdFromClaims, out int userId);

			if (parsedSuccessfully)
            {
				return userId;
            }

			throw new InvalidTokenException();
        }
	}
}

