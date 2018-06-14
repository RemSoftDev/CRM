using CRM.DAL.Entities;
using CRM.Report.DataProviders;
using System.Collections.Generic;
using System.ComponentModel;

namespace CRM.Report
{
	[DataObject]
	public class LeadsSource
	{
		[DataObjectMethod(DataObjectMethodType.Select)]
		public IEnumerable<Lead> GetLeads()
		{
			return DataProvider<Lead>.GetData();
		}
	}
}
