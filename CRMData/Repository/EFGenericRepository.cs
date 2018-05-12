using CRM.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace CRM.DAL.Repository
{
	public class EfGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
	{
		private readonly BaseContext _context;
		readonly DbSet<TEntity> _dbSet;

		public EfGenericRepository(BaseContext context)
		{
			_context = context;
			_dbSet = context.Set<TEntity>();
		}

		public IEnumerable<TEntity> Get()
		{
			return _dbSet.AsNoTracking().ToList();
		}

		public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
		{
			return _dbSet.AsNoTracking().Where(predicate).ToList();
		}
		public TEntity FindById(int id)
		{
			return _dbSet.Find(id);
		}

		public void Create(TEntity item)
		{
			_dbSet.Add(item);
		}
		public void Update(TEntity item)
		{
			_context.Entry(item).State = EntityState.Modified;
		}
		public void Remove(TEntity item)
		{
			_dbSet.Remove(item);
		}

		public IEnumerable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties)
		{
			return Include(includeProperties).ToList();
		}

		public IEnumerable<TEntity> GetWithInclude(
			Func<TEntity, bool> predicate,
			params Expression<Func<TEntity, object>>[] includeProperties)
		{
			var query = Include(includeProperties);
			return query.Where(predicate).ToList();
		}

		public void AddRange(IEnumerable<TEntity> entities)
		{
			_dbSet.AddRange(entities);
		}

		private IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
		{
			IQueryable<TEntity> query = _dbSet.AsNoTracking();
			return includeProperties
				.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
		}
	}
}
