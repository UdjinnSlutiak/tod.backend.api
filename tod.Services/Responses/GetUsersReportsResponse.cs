using System.Collections.Generic;
using Tod.Domain.Models.Enums;

namespace Tod.Services.Responses
{
	public class GetUsersReportsResponse
	{
        public List<UserReportData> UsersReports { get; set; }
    }
}

