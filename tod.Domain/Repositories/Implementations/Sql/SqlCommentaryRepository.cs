using System;
using System.Collections.Generic;
using Tod.Domain.Models;
using Tod.Domain.Repositories.Abstractions;
using Tod.Domain.Repositories.Realisations.Sql;

namespace Tod.Domain.Repositories.Implementations.Sql
{
	public class SqlCommentaryRepository : SqlBaseRepository<Commentary>, ICommentaryRepository
	{
        private readonly ProjectContext context;

		public SqlCommentaryRepository(ProjectContext context)
            : base(context)
		{
            this.context = context;
		}
    }
}

