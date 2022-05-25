using System;
using Tod.Domain.Models;
using Tod.Domain.Repositories.Abstractions;
using Tod.Domain.Repositories.Realisations.Sql;

namespace Tod.Domain.Repositories.Implementations.Sql
{
	public class SqlCommentaryReportRepository : SqlBaseRepository<CommentaryReport>, ICommentaryReportRepository
	{
		private readonly ProjectContext context;

		public SqlCommentaryReportRepository(ProjectContext context)
			: base(context)
		{
		}
	}
}

