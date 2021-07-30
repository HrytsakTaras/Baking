using Baking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baking.IRepositories
{
	public interface IGenericRepository<TEntity> where TEntity : class
	{
        Task Create(TEntity item);
        Task<TEntity> GetById(int id);
        Task<IEnumerable<TEntity>> GetAll();
        Task Delete(TEntity item);
        Task Update(int id, TEntity item);
        Task<TEntity> FindAsync(int id);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> Include(params Expression<Func<TEntity, object>>[] includes);
    }
}
