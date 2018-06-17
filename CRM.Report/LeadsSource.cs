using CRM.DAL.Entities;
using CRM.Report.DataProviders;
using System.Collections.Generic;
using System.ComponentModel;
using CRM.DAL.Repository;

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
