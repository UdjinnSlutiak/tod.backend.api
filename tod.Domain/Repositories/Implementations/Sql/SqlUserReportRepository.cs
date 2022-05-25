using System;
using Tod.Domain.Models;
using Tod.Domain.Repositories.Abstractions;
using Tod.Domain.Repositories.Realisations.Sql;

namespace Tod.Domain.Repositories.Implementations.Sql
{
	public class SqlUserReportRepository : SqlBaseRepository<UserReport>, IUserReportRepository
	{
		private readonly ProjectContext context;

		public SqlUserReportRepository(ProjectContext context)
			: base(context)
		{
			this.context = context;
		}
	}
}

