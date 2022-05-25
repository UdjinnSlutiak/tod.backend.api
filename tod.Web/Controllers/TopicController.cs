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
using Tod.Web.Extensions;

namespace Tod.Web.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/topics")]
	public class TopicController : ControllerBase
	{
		private readonly ITopicService topicService;

		public TopicController(ITopicService topicService)
		{
			this.topicService = topicService;
		}

		[AllowAnonymous]
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(TopicData), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<TopicData>> GetTopic(int id)
        {
			var userId = 0;
			try
            {
				userId = base.HttpContext.GetCurrentUserId();
            }
			catch (InvalidTokenException) { }

			try
            {
				var response = await this.topicService.GetTopicByIdAsync(userId, id);

				return response;
            }
			catch (NotFoundException ex)
            {
				return BadRequest(ex.Message);
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

		[AllowAnonymous]
		[HttpGet]
		[ProducesResponseType(typeof(GetTopicsResponse), StatusCodes.Status200OK)]
		public async Task<ActionResult<GetTopicsResponse>> GetTopics([FromQuery] int skip, int offset)
        {
			var userId = 0;
			try
            {
				userId = base.HttpContext.GetCurrentUserId();
			}
			catch (InvalidTokenException) { }

			var response = await this.topicService.GetTopicsAsync(userId, skip, offset);

			return response;
        }

		[HttpGet("favorites")]
		[ProducesResponseType(typeof(GetTopicsResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<GetTopicsResponse>> GetFavorites()
		{
			try
            {
				var userId = base.HttpContext.GetCurrentUserId();
				var response = await this.topicService.GetFavoritesAsync(userId);

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
			catch (DeletedContentException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpGet("discussed")]
		[ProducesResponseType(typeof(GetTopicsResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<GetTopicsResponse>> GetDiscussedTopics()
        {
			try
            {
				var userId = base.HttpContext.GetCurrentUserId();
				var response = await this.topicService.GetDiscussedTopicsAsync(userId);

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
			catch (DeletedContentException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpGet("my")]
		[ProducesResponseType(typeof(GetTopicsResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<GetTopicsResponse>> GetMyTopics()
        {
			try
            {
				var userId = base.HttpContext.GetCurrentUserId();
				var response = await this.topicService.GetMyTopicsAsync(userId);

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
			catch (DeletedContentException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpGet("recommended")]
		[ProducesResponseType(typeof(GetTopicsResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<GetTopicsResponse>> GetRecommendedTopics()
        {
			try
			{
				var userId = base.HttpContext.GetCurrentUserId();
				var response = await this.topicService.GetRecommendedTopicsAsync(userId);

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
			catch (DeletedContentException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpPost]
		[ProducesResponseType(typeof(CreateTopicResponse), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<CreateTopicResponse>> CreateTopic(CreateTopicRequest request)
        {
			try
            {
				var userId = base.HttpContext.GetCurrentUserId();

				var response = await this.topicService.CreateAsync(request, userId);

				return response;
			}
			catch (TopicAlreadyExistsException ex)
            {
				return BadRequest(ex.Message);
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
		}

		[HttpPut("{id}")]
		[ProducesResponseType(typeof(TopicData), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<TopicData>> UpdateTopic(int id, [FromBody] CreateTopicRequest request)
        {
			try
			{
				var userId = base.HttpContext.GetCurrentUserId();

				var response = await this.topicService.UpdateAsync(userId, id, request);

				return response;
			}
			catch (PermissionDeniedException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (TopicAlreadyExistsException ex)
			{
				return BadRequest(ex.Message);
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
		}

		[HttpPost("{id}/favorites")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult> AddToFavorites(int id)
        {
			try
            {
				int userId = base.HttpContext.GetCurrentUserId();

				var addedToFavorites = await this.topicService.AddToFavoritesAsync(id, userId);

				if (!addedToFavorites)
                {
					return BadRequest();
                }

				return Ok();
            }
			catch (TopicAlreadyInFavoritesException ex)
            {
				return BadRequest(ex.Message);
            }
			catch (ContentBelongsToYouException ex)
            {
				return BadRequest(ex.Message);
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
		}

		[AllowAnonymous]
		[HttpPost("search")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<GetTopicsResponse>> SearchTopics(TopicSearchRequest request)
        {
			try
            {
				var response = await this.topicService.SearchTopicsAsync(request);

				if (response == null)
                {
					return BadRequest();
                }

				return response;
            }
			catch (NotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult> DeleteTopic(int id)
        {
			try
            {
				var userId = base.HttpContext.GetCurrentUserId();
				await this.topicService.MarkTopicDeletedAsync(userId, id);
				return NoContent();
            }
			catch (PermissionDeniedException ex)
            {
				return BadRequest(ex.Message);
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
		}
	}
}

