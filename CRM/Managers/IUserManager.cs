using CRM.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace CRM.Managers
{
	public interface IUserManager
	{
		IdentityResult Create(User user, string password);
		User GetUser(string email, string password);
	}
}