using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tod.Services.Abstractions;
using Tod.Services.Exceptions;
using Tod.Services.Requests;
using Tod.Services.Responses;
using Tod.Web.Extensions;

namespace Tod.Web.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/topics/{topicId}/commentaries")]
	public class CommentaryController : ControllerBase
	{
		private readonly ICommentaryService commentaryService;

		public CommentaryController(ICommentaryService commentaryService)
		{
			this.commentaryService = commentaryService;
		}

		[AllowAnonymous]
		[HttpGet]
		[ProducesResponseType(typeof(GetCommentariesResponse), StatusCodes.Status200OK)]
		public async Task<ActionResult<GetCommentariesResponse>> GetTopicCommentariesAsync([FromRoute] int topicId)
        {
			var response = await this.commentaryService.GetCommentariesAsync(topicId);

			return response;
        }

		[HttpPost]
		[ProducesResponseType(typeof(CommentaryData), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<CommentaryData>> CreateCommentaryAsync([FromRoute] int topicId,
			[FromBody] CreateCommentaryRequest request)
        {
			try
            {
				var userId = base.HttpContext.GetCurrentUserId();

				var response = await this.commentaryService.CreateCommentaryAsync(topicId, userId, request.Text);

				return response;
            }
			catch (InvalidTokenException ex)
            {
				return Unauthorized(ex.Message);
            }
			catch (NotFoundException ex)
            {
				return Unauthorized(ex.Message);
            }
			catch (BannedContentException ex)
            {
				return NotFound(ex.Message);
            }
        }
	}
}

