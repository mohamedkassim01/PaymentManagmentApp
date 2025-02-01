using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace SystemPayment.API.Repositories.Interface
{
	public interface IRepository<T> where T : class
	{
		Task<T?> GetByIdAsync(int id);
		Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
		Task<IEnumerable<T>> GetAllAsync(); 
		Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
		Task<IEnumerable<T>> FindListAsync(Expression<Func<T, bool>> predicate);
		Task<IEnumerable<T>> NoTrackingFindAsync(Expression<Func<T, bool>> predicate);
		Task<bool> IsExist(Expression<Func<T, bool>> predicate);
		Task AddAsync(T entity);
		Task AddRangeAsync(IEnumerable<T> entities);
		void Update(T entity);
		void UpdateRange(IEnumerable<T> entities);
		void Remove(T entity);
		void RemoveRange(IEnumerable<T> entities);
	}
}
