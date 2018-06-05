using CRM.DAL.Contexts;
using CRM.Log;
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

			Logger.InitLog(typeof(EfGenericRepository<TEntity>));
		}

		public TEntity FindBy(Func<TEntity, bool> predicate)
		{
			Logger.SqlInfo("get item by predicate");
			return _dbSet.FirstOrDefault(predicate);
		}

		public IEnumerable<TEntity> Get()
		{
			Logger.SqlInfo("get all data in table");
			return _dbSet.AsNoTracking().ToList();
		}

		public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
		{
			Logger.SqlInfo("get data in table by predicate");
			return _dbSet.AsNoTracking().Where(predicate).ToList();
		}

		public TEntity FindById(int id)
		{
			Logger.SqlInfo($"get item by id = {id}");
			return _dbSet.Find(id);
		}

		public void Create(TEntity item)
		{
			Logger.SqlInfo($"insert item");
			_dbSet.Add(item);
		}

		public void Update(TEntity item)
		{
			Logger.SqlInfo($"update item");
			_context.Entry(item).State = EntityState.Modified;
		}

		public bool Any(Func<TEntity, bool> predicate)
		{
			Logger.SqlInfo($"is exist item(s) by predicate");
			return _dbSet.Any(predicate);
		}

		public void UpdateRange(IEnumerable<TEntity> items)
		{
			foreach (var item in items)
			{
				Update(item);
			}
		}

		public void Remove(TEntity item)
		{
			Logger.SqlInfo($"remove item");
			_dbSet.Remove(item);
		}

		public IEnumerable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties)
		{
			Logger.SqlInfo($"get with include");
			return Include(includeProperties).ToList();
		}

		public IEnumerable<TEntity> GetWithInclude(
			Func<TEntity, bool> predicate,
			params Expression<Func<TEntity, object>>[] includeProperties)
		{
			Logger.SqlInfo($"get with include");
			var query = Include(includeProperties);
			return query.Where(predicate).ToList();
		}

		public void AddRange(IEnumerable<TEntity> entities)
		{
			Logger.SqlInfo($"Sql inset {entities.Count()} items");
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
