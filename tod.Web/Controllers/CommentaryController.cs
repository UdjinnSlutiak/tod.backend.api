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
	[Route("api")]
	public class CommentaryController : ControllerBase
	{
		private readonly ICommentaryService commentaryService;

		public CommentaryController(ICommentaryService commentaryService)
		{
			this.commentaryService = commentaryService;
		}

		[AllowAnonymous]
		[HttpGet("topics/{topicId}/commentaries")]
		[ProducesResponseType(typeof(GetCommentariesResponse), StatusCodes.Status200OK)]
		public async Task<ActionResult<GetCommentariesResponse>> GetTopicCommentaries([FromRoute] int topicId)
        {
			try
            {
				var response = await this.commentaryService.GetTopicCommentariesAsync(topicId);

				return response;
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

		[HttpPost("topics/{topicId}/commentaries")]
		[ProducesResponseType(typeof(CommentaryData), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<CommentaryData>> CreateCommentary([FromRoute] int topicId,
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

		[HttpDelete("commentaries/{id}")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult> DeleteCommentary(int id)
		{
			try
			{
				var userId = base.HttpContext.GetCurrentUserId();
				await this.commentaryService.MarkCommentaryDeletedAsync(userId, id);
				return NoContent();
			}
			catch (NotFoundException ex)
			{
				return Unauthorized(ex.Message);
			}
			catch (InvalidTokenException ex)
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
			catch (PermissionDeniedException ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}

