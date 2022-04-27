using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tod.Domain.Models;
using Tod.Domain.Models.Dtos;
using Tod.Domain.Repositories.Abstractions;

namespace Tod.Domain.Repositories.Realisations.Sql
{
	public class SqlUserRepository : SqlBaseRepository<User>, IUserRepository
	{
        private readonly ProjectContext context;

        public SqlUserRepository(ProjectContext context)
            : base(context)
        {
            this.context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await this.context.Set<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await this.context.Set<User>().Where(u => u.Username == username).FirstOrDefaultAsync();
        }
    }
}

