using CRM.DAL.Entities;
using CRM.DAL.Repository;
using System.Collections.Generic;
using System.ComponentModel;

namespace CRM.Report
{
	[DataObject]
	public class LeadsSource
	{
		private UnitOfWork _unitOfWork;

		public LeadsSource()
		{
			_unitOfWork = new UnitOfWork();
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IEnumerable<Lead> GetLeads()
		{
			return _unitOfWork.LeadsRepository.Get();
		}
	}
}
