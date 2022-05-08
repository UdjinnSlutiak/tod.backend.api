using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tod.Domain.Models;

namespace Tod.Domain.Repositories.Abstractions
{
	public interface ITagRepository : IRepository<Tag>
	{
		public Task<Tag> GetByTitleAsync(string title);
	}
}

