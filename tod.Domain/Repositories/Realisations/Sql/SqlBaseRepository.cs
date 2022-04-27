using System.Linq;
using System.Threading.Tasks;
using Tod.Domain.Repositories.Abstractions;

namespace Tod.Domain.Repositories.Realisations.Sql
{
    public class SqlBaseRepository<T> : IRepository<T>
        where T : class
    {
        private readonly ProjectContext context;

        public SqlBaseRepository(ProjectContext context)
        {
            this.context = context;
        }

        public async Task<T> GetAsync(int id)
        {
            return await this.context.Set<T>().FindAsync(id);
        }

        public IQueryable<T> GetRange(int skip, int offset)
        {
            return this.context.Set<T>().Skip(skip).Take(offset).AsQueryable();
        }

        public async Task<T> CreateAsync(T entity)
        {
            await this.context.AddAsync(entity);
            return entity;
        }

        public T Update(T entity)
        {
            this.context.Update(entity);
            return entity;
        }

        public void Delete(int id)
        {
            this.context.Remove(id);
        }
    }
}

