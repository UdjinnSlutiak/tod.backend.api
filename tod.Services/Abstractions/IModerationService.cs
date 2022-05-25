using System;
using System.Threading.Tasks;
using Tod.Services.Requests;
using Tod.Services.Responses;

namespace Tod.Services.Abstractions
{
	public interface IModerationService
	{
		public Task<GetUsersReportsResponse> GetUsersReportsAsync(int skip = 0, int offset = 20);
		public Task<GetTopicsReportsResponse> GetTopicsReportsAsync(int skip = 0, int offset = 20);
		public Task<GetCommentariesReportsResponse> GetCommentariesReportsAsync(int skip = 0, int offset = 20);
		public Task ReportOnUserAsync(int reporterId, int userId, ReportRequest request);
		public Task ReportOnTopicAsync(int reporterId, int topicId, ReportRequest request);
		public Task ReportOnCommentaryAsync(int reporterId, int commentaryId, ReportRequest request);
		public Task BanUserAsync(int adminId, int userId);
		public Task BanTopicAsync(int adminId, int topicId);
		public Task BanCommentaryAsync(int adminId, int commentaryId);
	}
}

