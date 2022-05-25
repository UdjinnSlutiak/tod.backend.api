using System;
using Tod.Domain.Models;
using Tod.Domain.Repositories.Abstractions;
using Tod.Domain.Repositories.Realisations.Sql;

namespace Tod.Domain.Repositories.Implementations.Sql
{
	public class SqlTopicReportRepository : SqlBaseRepository<TopicReport>, ITopicReportRepository
	{
		private readonly ProjectContext context;

		public SqlTopicReportRepository(ProjectContext context)
			: base(context)
		{
		}
	}
}

