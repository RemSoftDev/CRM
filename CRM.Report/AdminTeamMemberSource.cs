using CRM.DAL.Entities;
using CRM.DAL.Repository;
using System.Collections.Generic;
using System.ComponentModel;

namespace CRM.Report
{
	[DataObject]
	public class AdminTeamMemberSource
	{
		private readonly UnitOfWork _unitOfWork;

		public AdminTeamMemberSource()
		{
			_unitOfWork = new UnitOfWork();
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IEnumerable<User> GetAdminTeamMember()
		{
			var adminTeamMemberType = _unitOfWork.DUserTypesRepository.AdminTeamMember;
			return _unitOfWork.UsersRepository.Get(u => u.UserTypeId == adminTeamMemberType.Id);
		}
	}
}
