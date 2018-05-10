using CRMData.Contexts;
using CRMData.Entities;
using System;

namespace CRMData.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly BaseContext _dbContext;

		private IGenericRepository<Lead> _leadsRepository;
		private IGenericRepository<Note> _notesRepository;

		public UnitOfWork()
		{
			_dbContext = new BaseContext();
		}

		public BaseContext Context => _dbContext;

		public IGenericRepository<Lead> LeadsRepository
		{
			get
			{
				if (_leadsRepository == null)
				{
					_leadsRepository = new EfGenericRepository<Lead>(_dbContext);
				}

				return _leadsRepository;
			}
		}

		public IGenericRepository<Note> NotesRepository
		{
			get
			{
				if (_notesRepository == null)
				{
					_notesRepository = new EfGenericRepository<Note>(_dbContext);
				}

				return _notesRepository;
			}
		}

		private bool _disposed;

		public void Save()
		{
			_dbContext.SaveChanges();
		}

		public virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_dbContext.Dispose();
				}

				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}