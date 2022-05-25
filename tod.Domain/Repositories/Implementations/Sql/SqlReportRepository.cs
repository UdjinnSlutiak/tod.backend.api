using System;
using Tod.Domain.Models;
using Tod.Domain.Repositories.Abstractions;
using Tod.Domain.Repositories.Realisations.Sql;

namespace Tod.Domain.Repositories.Implementations.Sql
{
	public class SqlReportRepository : SqlBaseRepository<Report>, IReportRepository
	{
		private readonly ProjectContext context;

		public SqlReportRepository(ProjectContext context)
			: base(context)
		{
			this.context = context;
		}
	}
}

