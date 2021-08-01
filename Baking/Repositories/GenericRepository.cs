using Baking.Data;
using Baking.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baking.Repositories
{
	public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
	{
		private readonly ApplicationContext _context;
		DbSet<TEntity> _dbSet;

		public GenericRepository(ApplicationContext context)
		{
			_context = context;
			_dbSet = context.Set<TEntity>();
		}

		public async Task Create(TEntity item)
		{
			_dbSet.Add(item);
			await _context.SaveChangesAsync();
		}

		public async Task Delete(TEntity item)
		{
			_dbSet.Remove(item);
			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<TEntity>> GetAll()
		{
			return await _dbSet.ToListAsync(); 
		}

		public async Task<TEntity> GetById(int id)
		{
			var result = await _dbSet.FindAsync(id);
			return result as TEntity;
		}

		public async Task Update(int id, TEntity item)
		{
			_context.Entry(item).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}

		public async Task<TEntity> FindAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity,bool>> filter)
		{
			IQueryable<TEntity> queryable = _dbSet;
			return await queryable.Where(filter).ToListAsync();			
		}

		public IEnumerable<TEntity> Include(params Expression<Func<TEntity, object>>[] includes)
		{

			IEnumerable<TEntity> query = null;
			foreach (var include in includes)
			{
				query = _dbSet.Include(include);
			}

			return query ?? _dbSet;
		}
	}
}
