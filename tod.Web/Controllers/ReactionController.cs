using System.Collections.Generic;
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
	[ApiController]
	[Authorize]
	[Route("api/reactions")]
	public class ReactionController : ControllerBase
	{
		private readonly IReactionService reactionService;

		public ReactionController(IReactionService reactionService)
		{
			this.reactionService = reactionService;
		}

		[HttpPost("topics/{topicId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> ReactOnTopicAsync(int topicId, [FromBody] ReactionRequest request)
        {
			try
            {
				var userId = base.HttpContext.GetCurrentUserId();

				var reactedSuccessfully = await this.reactionService.ReactOnTopicAsync(topicId, userId, request.Value);

				if (reactedSuccessfully)
                {
					return BadRequest();
                }

				return Ok();
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
			catch (ContentBelongsToYouException ex)
            {
				return BadRequest(ex.Message);
            }
			catch (AlreadyReactedException ex)
            {
				return BadRequest(ex.Message);
            }
		}

		[HttpPost("commentaries/{commentaryId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> ReactOnCommentaryAsync(int commentaryId, [FromBody] ReactionRequest request)
        {
			try
			{
				var userId = base.HttpContext.GetCurrentUserId();

				var reactedSuccessfully = await this.reactionService.ReactOnCommentaryAsync(commentaryId, userId, request.Value);

				if (!reactedSuccessfully)
				{
					return BadRequest();
				}

				return Ok();
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
			catch (ContentBelongsToYouException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (AlreadyReactedException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("topics/{id}")]
		[ProducesResponseType(typeof(ContentReactionData), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ContentReactionData>> GetUserTopicReactionAsync(int id)
        {
			try
            {
				var userId = base.HttpContext.GetCurrentUserId();
				var response = await this.reactionService.GetUserTopicReactionByTopicId(userId, id);

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

		[HttpGet("topics/{id}/commentaries")]
		[ProducesResponseType(typeof(List<ContentReactionData>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<List<ContentReactionData>>> GetUserCommentariesReactionAsync(int id)
		{
			try
			{
				var userId = base.HttpContext.GetCurrentUserId();
				var response = await this.reactionService.GetUserCommentariesReactions(userId, id);

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

