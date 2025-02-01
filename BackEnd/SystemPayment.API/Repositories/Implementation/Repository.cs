﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SystemPayment.API.DataModels;
using SystemPayment.API.Repositories.Interface;

namespace SystemPayment.API.Repositories.Implementation
{
	public class Repository<T> : IRepository<T> where T : class
	{
		protected readonly ApplicationDbContext _context;
		protected readonly DbSet<T> _dbSet;

		public Repository(ApplicationDbContext context)
		{
			_context = context;
			_dbSet = context.Set<T>();
		}

		public async Task<T?> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}
		public async Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
		{
			return await _dbSet.Where(predicate).FirstOrDefaultAsync();
		}
		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _dbSet.AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
		{
			var query = _dbSet.AsNoTracking().Where(predicate).AsQueryable();
			foreach (var include in includes) query = query.Include(include);
			return await query.ToListAsync(); ;
		}

		public async Task<IEnumerable<T>> NoTrackingFindAsync(Expression<Func<T, bool>> predicate)
		{
			return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
		}

		public async Task<IEnumerable<T>> FindListAsync(Expression<Func<T, bool>> predicate)
		{
			return await _dbSet.Where(predicate).ToListAsync();
		}
		public async Task AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
		}

		public async Task AddRangeAsync(IEnumerable<T> entities)
		{
			await _dbSet.AddRangeAsync(entities);
		}

		public void Update(T entity)
		{
			_dbSet.Update(entity);
		}
		public void UpdateRange(IEnumerable<T> entities)
		{
			_dbSet.UpdateRange(entities);
		}

		public void Remove(T entity)
		{
			_dbSet.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			_dbSet.RemoveRange(entities);
		}

		public async Task<bool> IsExist(Expression<Func<T, bool>> predicate)
		{
			return await _dbSet.AsNoTracking().AnyAsync(predicate);
		}
	}
}
