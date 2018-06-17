using CRM.DAL.Entities;
using CRM.DAL.Repository;
using System.Collections.Generic;
using System.ComponentModel;

namespace CRM.Report
{
	[DataObject]
	public class CustomerSource
	{
		private readonly UnitOfWork _unitOfWork;

		public CustomerSource()
		{
			_unitOfWork = new UnitOfWork();
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IEnumerable<User> GetCustomer()
		{
			var customerType = _unitOfWork.DUserTypesRepository.Customer;
			return _unitOfWork.UsersRepository.Get(u => u.UserTypeId == customerType.Id);
		}
	}
}
