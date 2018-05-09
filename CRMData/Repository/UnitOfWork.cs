using System;
using CRMData.Contexts;
using CRMData.Entities;

namespace CRMData.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private BaseContext _dbContext;

		private EFGenericRepository<Lead> _leadsRepository;

		public UnitOfWork()
		{
			_dbContext = new BaseContext();
		}

		public UnitOfWork(BaseContext context)
		{
			_dbContext = context;
		}

		public BaseContext Context => _dbContext;

		public EFGenericRepository<Lead> LeadsRepository
		{
			get
			{
				if (_leadsRepository == null)
				{
					_leadsRepository = new EFGenericRepository<Lead>(_dbContext);
				}

				return _leadsRepository;
			}
		}

		private bool disposed = false;

		public virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					_dbContext.Dispose();
				}

				this.disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}