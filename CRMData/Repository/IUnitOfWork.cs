using CRMData.Contexts;
using CRMData.Entities;
using System;

namespace CRMData.Repository
{
	public interface IUnitOfWork : IDisposable
	{
		BaseContext Context { get; }
		EFGenericRepository<Lead> LeadsRepository { get; }
	}
}