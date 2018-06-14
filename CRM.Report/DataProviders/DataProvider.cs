using System.Collections.Generic;

namespace CRM.Report.DataProviders
{
	public static class DataProvider<T>
	{
		private static IEnumerable<T> _dataItems;

		public static void SetData(IEnumerable<T> data)
		{
			_dataItems = data;
		}

		public static IEnumerable<T> GetData()
		{
			return _dataItems;
		}
	}
}
