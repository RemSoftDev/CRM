using CRM.DAL.Entities;
using CRM.DAL.Repository;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;

namespace CRM.Managers
{
	public class LeadManager : ILeadManager
	{
		private readonly IUnitOfWork _unitOfWork;

		public LeadManager(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IdentityResult Create(Lead lead)
		{
			var errors = ValidateAndGetError(lead);


			// ReSharper disable once PossibleMultipleEnumeration
			if (!errors.Any())
			{

				_unitOfWork.LeadsRepository.Create(lead);
				_unitOfWork.Save();

				return IdentityResult.Success;
			}

			// ReSharper disable once PossibleMultipleEnumeration
			return new IdentityResult(errors);
		}

		private IEnumerable<string> ValidateAndGetError(Lead lead)
		{
			if (IsEmailExist(lead.Email))
			{
				yield return $"Email {lead.Email} alrady exist";
			}

			if (IsLeadNameExist(lead.Name))
			{
				yield return $"Lead with name{lead.Name} alrady exist";
			}
		}

		private bool IsLeadNameExist(string name)
		{
			return _unitOfWork.LeadsRepository.Any(u => u.Name == name);
		}

		private bool IsEmailExist(string email)
		{
			return _unitOfWork.LeadsRepository.Any(u => u.Email == email);
		}

		public Lead GetLead(string email)
		{
			return _unitOfWork.LeadsRepository
				.Get(u => u.Email == email)
				.FirstOrDefault();
		}
	}
}