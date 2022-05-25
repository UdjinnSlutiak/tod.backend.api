using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;
using Tod.Domain.Models.Dtos;
using Tod.Domain.Models.Enums;
using Tod.Domain.Repositories.Abstractions;
using Tod.Services.Abstractions;
using Tod.Services.Exceptions;
using Tod.Services.Requests;
using Tod.Services.Responses;

namespace Tod.Services.Implementations
{
	public class ModerationService : IModerationService
	{
		private readonly IReportRepository reportRepository;
		private readonly IUserReportRepository userReportRepository;
		private readonly ITopicReportRepository topicReportRepository;
		private readonly ICommentaryReportRepository commentaryReportRepository;
		private readonly IUserService userService;
		private readonly ITopicService topicService;
		private readonly ICommentaryService commentaryService;
		private readonly IContentValidator contentValidator;

		public ModerationService(IReportRepository reportRepository,
			IUserReportRepository userReportRepository,
			ITopicReportRepository topicReportRepository,
			ICommentaryReportRepository commentaryReportRepository,
			IUserService userService,
			ITopicService topicService,
			ICommentaryService commentaryService,
			IContentValidator contentValidator)
		{
			this.reportRepository = reportRepository;
			this.userReportRepository = userReportRepository;
			this.topicReportRepository = topicReportRepository;
			this.commentaryReportRepository = commentaryReportRepository;
			this.userService = userService;
			this.topicService = topicService;
			this.commentaryService = commentaryService;
			this.contentValidator = contentValidator;
		}

		public async Task<GetUsersReportsResponse> GetUsersReportsAsync(int skip = 0, int offset = 20)
        {
			var usersReports = this.userReportRepository.GetRange(skip, offset);

			var usersReportsData = new List<UserReportData>();
			foreach (var userReport in usersReports)
            {
				var user = await this.userService.GetByIdAsync(userReport.UserId);
				var report = await this.reportRepository.GetAsync(userReport.ReportId);
				var userReportData = new UserReportData
				{
					User = new UserDto(user),
					Type = report.Type.ToString(),
					Description = report.Description
				};
				usersReportsData.Add(userReportData);
            }

			return new GetUsersReportsResponse
			{
				UsersReports = usersReportsData
			};
        }

		public async Task<GetTopicsReportsResponse> GetTopicsReportsAsync(int skip = 0, int offset = 20)
        {
			var topicsReports = this.topicReportRepository.GetRange(skip, offset);

			var topicsReportsData = new List<TopicReportData>();
			foreach(var topicReport in topicsReports)
            {
				var topic = await this.topicService.GetTopicByIdAsync(0, topicReport.TopicId);
				var report = await this.reportRepository.GetAsync(topicReport.ReportId);
				var topicReportData = new TopicReportData
				{
					Topic = topic,
					Type = report.Type.ToString(),
					Description = report.Description
				};
				topicsReportsData.Add(topicReportData);
            }

			return new GetTopicsReportsResponse
			{
				TopicsReports = topicsReportsData
			};
        }

		public async Task<GetCommentariesReportsResponse> GetCommentariesReportsAsync(int skip = 0, int offset = 20)
        {
			var commentariesReports = this.commentaryReportRepository.GetRange(skip, offset);

			var commentariesReportsData = new List<CommentaryReportData>();
			foreach (var commentaryReport in commentariesReports)
            {
				var commentary = await this.commentaryService.GetDataByIdAsync(commentaryReport.CommentaryId);
				var report = await this.reportRepository.GetAsync(commentaryReport.ReportId);
				var commentaryReportData = new CommentaryReportData
				{
					Commentary = commentary,
					Type = report.Type.ToString(),
					Description = report.Description
				};
				commentariesReportsData.Add(commentaryReportData);
            }

			return new GetCommentariesReportsResponse
			{
				CommentariesReports = commentariesReportsData
			};
        }

		public async Task ReportOnUserAsync(int reporterId, int userId, ReportRequest request)
        {
			var reporter = new User();
			try
            {
				reporter = await this.contentValidator.GetAndValidateUserAsync(reporterId);
			}
			catch (BannedContentException) { }

			var user = await this.contentValidator.GetAndValidateUserAsync(userId);

			var report = new Report
			{
				Type = request.Type,
				Description = request.Description
			};
			report = await this.reportRepository.CreateAsync(report);

			var userReport = new UserReport
			{
				UserId = userId,
				ReportId = report.Id
			};
			await this.userReportRepository.CreateAsync(userReport);
        }

		public async Task ReportOnTopicAsync(int reporterId, int topicId, ReportRequest request)
		{
			var reporter = new User();
			try
			{
				reporter = await this.contentValidator.GetAndValidateUserAsync(reporterId);
			}
			catch (BannedContentException) { }

			var topic = await this.contentValidator.GetAndValidateTopicAsync(topicId);

			var report = new Report
			{
				Type = request.Type,
				Description = request.Description
			};
			report = await this.reportRepository.CreateAsync(report);

			var topicReport = new TopicReport
			{
				TopicId = topicId,
				ReportId = report.Id
			};
			await this.topicReportRepository.CreateAsync(topicReport);
		}

		public async Task ReportOnCommentaryAsync(int reporterId, int commentaryId, ReportRequest request)
		{
			var reporter = new User();
			try
			{
				reporter = await this.contentValidator.GetAndValidateUserAsync(reporterId);
			}
			catch (BannedContentException) { }

			var commentary = await this.contentValidator.GetAndValidateCommentaryAsync(commentaryId);

			var report = new Report
			{
				Type = request.Type,
				Description = request.Description
			};
			report = await this.reportRepository.CreateAsync(report);

			var commentaryReport = new CommentaryReport
			{
				CommentaryId = commentaryId,
				ReportId = report.Id
			};
			await this.commentaryReportRepository.CreateAsync(commentaryReport);
		}

		public async Task BanUserAsync(int adminId, int userId)
        {
			var admin = await this.contentValidator.GetAndValidateUserAsync(adminId);
			if (admin.Role != Role.Administrator || admin.Role != Role.Moderator)
            {
				throw new PermissionDeniedException();
            }

			var user = await this.userService.GetByIdAsync(userId);
			if (user == null)
            {
				throw new NotFoundException(ContentType.User);
            }

			user.Status = ContentStatus.Banned;
			await this.userService.UpdateAsync(user);
        }

		public async Task BanTopicAsync(int adminId, int topicId)
        {
			var admin = await this.contentValidator.GetAndValidateUserAsync(adminId);
			if (admin.Role != Role.Administrator || admin.Role != Role.Moderator)
			{
				throw new PermissionDeniedException();
			}

			var topic = await this.topicService.GetByIdAsync(topicId);
			if (topic == null)
            {
				throw new NotFoundException(ContentType.Topic);
            }

			topic.Status = ContentStatus.Banned;
			await this.topicService.UpdateAsync(topic);
		}

		public async Task BanCommentaryAsync(int adminId, int commentaryId)
        {
			var admin = await this.contentValidator.GetAndValidateUserAsync(adminId);
			if (admin.Role != Role.Administrator || admin.Role != Role.Moderator)
			{
				throw new PermissionDeniedException();
			}

			var commentary = await this.commentaryService.GetByIdAsync(commentaryId);
			if (commentary == null)
            {
				throw new NotFoundException(ContentType.Commentary);
            }

			commentary.Status = ContentStatus.Banned;
			await this.commentaryService.UpdateCommentaryAsync(commentary);
		}
	}
}

