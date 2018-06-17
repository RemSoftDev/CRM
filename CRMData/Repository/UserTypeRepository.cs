using CRM.DAL.Contexts;
using CRM.DAL.Entities;

namespace CRM.DAL.Repository
{
	public interface IUserTypeRepository
	{

	}

	public class UserTypeRepository : EfGenericRepository<DUserType>, IUserTypeRepository
	{
		public UserTypeRepository(BaseContext context) : base(context)
		{
		}

		public DUserType AdminTeamMember => FindBy(d => d.TypeName == "AdminTeamMember");
		public DUserType Customer => FindBy(d => d.TypeName == "Customer");
	}
}
