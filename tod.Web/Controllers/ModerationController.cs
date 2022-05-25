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
	[Authorize(Roles = "Administrator,Moderator")]
	[ApiController]
	[Route("api/moderation")]
	public class ModerationController : ControllerBase
	{
		private readonly IModerationService moderationService;

		public ModerationController(IModerationService moderationService)
		{
			this.moderationService = moderationService;
		}

		[HttpGet("reports/users")]
		[ProducesResponseType(typeof(GetUsersReportsResponse), StatusCodes.Status200OK)]
		public async Task<ActionResult<GetUsersReportsResponse>> GetUsersReports()
        {
			var response = await this.moderationService.GetUsersReportsAsync();
			return response;
        }

		[HttpGet("reports/topics")]
		[ProducesResponseType(typeof(GetTopicsReportsResponse), StatusCodes.Status200OK)]
		public async Task<ActionResult<GetTopicsReportsResponse>> GetTopicsReports()
		{
			var response = await this.moderationService.GetTopicsReportsAsync();
			return response;
		}

		[HttpGet("reports/commentaries")]
		[ProducesResponseType(typeof(GetCommentariesReportsResponse), StatusCodes.Status200OK)]
		public async Task<ActionResult<GetCommentariesReportsResponse>> GetCommentariesReports()
		{
			var response = await this.moderationService.GetCommentariesReportsAsync();
			return response;
		}

        [HttpPost("reports/users/{id}")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult> ReportOnUser([FromQuery] int id, [FromBody] ReportRequest request)
        {
			try
            {
				var userId = base.HttpContext.GetCurrentUserId();
				await this.moderationService.ReportOnUserAsync(userId, id, request);

				return Ok();
            }
			catch (InvalidTokenException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (PermissionDeniedException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (NotFoundException ex)
			{
				return NotFound(ex.Message);
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

		[HttpPost("reports/topics/{id}")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult> ReportOnTopic([FromQuery] int id, [FromBody] ReportRequest request)
		{
			try
			{
				var userId = base.HttpContext.GetCurrentUserId();
				await this.moderationService.ReportOnTopicAsync(userId, id, request);

				return Ok();
			}
			catch (InvalidTokenException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (PermissionDeniedException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (NotFoundException ex)
			{
				return NotFound(ex.Message);
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

		[HttpPost("reports/commentaries/{id}")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult> ReportOnCommentary([FromQuery] int id, [FromBody] ReportRequest request)
		{
			try
			{
				var userId = base.HttpContext.GetCurrentUserId();
				await this.moderationService.ReportOnCommentaryAsync(userId, id, request);

				return Ok();
			}
			catch (InvalidTokenException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (PermissionDeniedException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (NotFoundException ex)
			{
				return NotFound(ex.Message);
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

		[HttpPost("users/{id}")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult> BanUser(int id)
        {
			try
            {
				var adminId = base.HttpContext.GetCurrentUserId();
				await this.moderationService.BanUserAsync(adminId, id);
				return Ok();
            }
			catch (InvalidTokenException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (PermissionDeniedException ex)
            {
				return BadRequest(ex.Message);
            }
			catch (NotFoundException ex)
            {
				return NotFound(ex.Message);
            }
			catch (DeletedContentException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpPost("topics/{id}")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult> BanTopic(int id)
		{
			try
			{
				var adminId = base.HttpContext.GetCurrentUserId();
				await this.moderationService.BanTopicAsync(adminId, id);
				return Ok();
			}
			catch (InvalidTokenException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (PermissionDeniedException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (NotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (DeletedContentException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpPost("commentaries/{id}")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult> BanCommentary(int id)
		{
			try
			{
				var adminId = base.HttpContext.GetCurrentUserId();
				await this.moderationService.BanCommentaryAsync(adminId, id);
				return Ok();
			}
			catch (InvalidTokenException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (PermissionDeniedException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (NotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (DeletedContentException ex)
			{
				return NotFound(ex.Message);
			}
		}
	}
}

