using Tod.Domain.Models.Dtos;

namespace Tod.Services.Responses
{
	public class UserReportData : ReportData
	{
        public UserDto User { get; set; }
    }
}

