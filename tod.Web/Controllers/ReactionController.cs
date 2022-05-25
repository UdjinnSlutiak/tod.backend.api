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
		public async Task<ActionResult> ReactOnTopic(int topicId, [FromBody] ReactionRequest request)
        {
			try
            {
				var userId = base.HttpContext.GetCurrentUserId();

				var reactedSuccessfully = await this.reactionService.ReactOnTopicAsync(topicId, userId, request.Value);

				if (!reactedSuccessfully)
                {
					return BadRequest();
                }

				return Ok();
            }
			catch (ContentBelongsToYouException ex)
            {
				return BadRequest(ex.Message);
            }
			catch (AlreadyReactedException ex)
            {
				return BadRequest(ex.Message);
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
			catch (DeletedContentException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpPost("commentaries/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> ReactOnCommentary(int id, [FromBody] ReactionRequest request)
        {
			try
			{
				var userId = base.HttpContext.GetCurrentUserId();

				var reactedSuccessfully = await this.reactionService.ReactOnCommentaryAsync(id, userId, request.Value);

				if (!reactedSuccessfully)
				{
					return BadRequest();
				}

				return Ok();
			}
			catch (ContentBelongsToYouException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (AlreadyReactedException ex)
			{
				return BadRequest(ex.Message);
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
			catch (DeletedContentException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpGet("topics/{id}")]
		[ProducesResponseType(typeof(ContentReactionData), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ContentReactionData>> GetUserTopicReaction(int id)
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
		public async Task<ActionResult<ContentReactionsResponse>> GetUserCommentariesReaction(int id)
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

		[HttpGet("commentaries/{id}")]
		[ProducesResponseType(typeof(List<ContentReactionData>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ContentReactionData>> GetUserCommentaryReaction(int id)
        {
			try
            {
				var userId = base.HttpContext.GetCurrentUserId();
				var response = await this.reactionService.GetUserCommentaryReaction(userId, id);

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

