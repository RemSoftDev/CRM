using CRMData.Contexts;
using CRMData.Entities;
using System;

namespace CRMData.Repository
{
	public interface IUnitOfWork : IDisposable
	{
		BaseContext Context { get; }
		IGenericRepository<Lead> LeadsRepository { get; }
		IGenericRepository<Note> NotesRepository { get; }
		void Save();
		void Dispose(bool disposing);
	}
}