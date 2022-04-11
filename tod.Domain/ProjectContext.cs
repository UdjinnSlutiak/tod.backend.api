using System;
using Microsoft.EntityFrameworkCore;

namespace tod.Domain
{
	public class ProjectContext : DbContext
	{
		public ProjectContext(DbContextOptions<ProjectContext> options)
			: base(options)
		{
			Database.EnsureCreated();
		}
	}
}
