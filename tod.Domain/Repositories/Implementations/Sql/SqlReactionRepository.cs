using System;
using Tod.Domain.Models;
using Tod.Domain.Repositories.Abstractions;
using Tod.Domain.Repositories.Realisations.Sql;

namespace Tod.Domain.Repositories.Implementations.Sql
{
	public class SqlReactionRepository : SqlBaseRepository<Reaction>, IReactionRepository
	{
		private readonly ProjectContext context;

		public SqlReactionRepository(ProjectContext context)
			: base(context)
		{
			this.context = context;
		}
	}
}

