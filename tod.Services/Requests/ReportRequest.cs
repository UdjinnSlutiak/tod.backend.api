using System;
using Tod.Domain.Models.Enums;

namespace Tod.Services.Requests
{
	public class ReportRequest
	{
        public ReportType Type { get; set; }
        public string Description { get; set; }
    }
}

