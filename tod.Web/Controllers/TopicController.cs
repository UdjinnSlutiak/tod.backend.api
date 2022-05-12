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
		public async Task<ActionResult<TopicData>> GetTopicAsync(int id)
        {
			try
            {
				var response = await this.topicService.GetTopicDataByIdAsync(id);

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
        }

		[AllowAnonymous]
		[HttpGet]
		[ProducesResponseType(typeof(GetTopicsResponse), StatusCodes.Status200OK)]
		public async Task<ActionResult<GetTopicsResponse>> GetTopicsAsync([FromQuery] int skip, int offset)
        {
			var response = await this.topicService.GetTopicsAsync(skip, offset);

			return response;
        }

		[HttpGet("favorites")]
		[ProducesResponseType(typeof(GetTopicsResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<GetTopicsResponse>> GetFavoritesAsync()
		{
			try
            {
				var userId = base.HttpContext.GetCurrentUserId();
				var response = await this.topicService.GetFavoritesAsync(userId);

				return response;
            }
			catch (NotFoundException ex)
			{
				return Unauthorized(ex.Message);
			}
			catch (InvalidTokenException ex)
			{
				return Unauthorized(ex.Message);
			}
		}

		[HttpPost]
		[ProducesResponseType(typeof(CreateTopicResponse), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<CreateTopicResponse>> CreateTopicAsync(CreateTopicRequest request)
        {
			try
            {
				var userId = base.HttpContext.GetCurrentUserId();

				var response = await this.topicService.CreateAsync(request, userId);

				return response;
			}
			catch (NotFoundException ex)
            {
				return Unauthorized(ex.Message);
            }
			catch (InvalidTokenException ex)
            {
				return Unauthorized(ex.Message);
			}
			catch (TopicAlreadyExistsException ex)
            {
				return BadRequest(ex.Message);
            }
        }

		[HttpPost("{id}/favorites")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult> AddToFavoritesAsync(int id)
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
			catch (TopicAlreadyInFavoritesException ex)
            {
				return BadRequest(ex.Message);
            }
			catch (ContentBelongsToYouException ex)
            {
				return BadRequest(ex.Message);
            }
        }

		[AllowAnonymous]
		[HttpPost("search")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<GetTopicsResponse>> SearchTopicsAsync(TopicSearchRequest request)
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
	}
}

