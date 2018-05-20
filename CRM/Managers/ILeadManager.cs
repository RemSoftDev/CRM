using CRM.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace CRM.Managers
{
	public interface ILeadManager
	{
		IdentityResult Create(Lead lead);
		Lead GetLead(string email);
	}
}