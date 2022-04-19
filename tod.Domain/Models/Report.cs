using Tod.Domain.Models.Enums;

namespace Tod.Domain.Models
{
	public class Report
	{
        public int Id { get; set; }
        public ReportType Type { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
}
