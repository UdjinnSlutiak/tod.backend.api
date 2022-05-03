using System.Linq;
using System.Threading.Tasks;

namespace Tod.Domain.Repositories.Abstractions
{
	public interface IRepository<T> where T : class
	{
		public Task<T> GetAsync(int id);

		public IQueryable<T> GetRange(int skip, int offset);

		public Task<T> CreateAsync(T entity);

		public Task<T> UpdateAsync(T entity);

		public Task DeleteAsync(int id);
	}
}
