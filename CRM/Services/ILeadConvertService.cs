using System.Collections.Generic;
using CRM.Models;

namespace CRM.Services
{
	public interface ILeadConvertService
	{
		void Convert(LeadConvertViewModel model, List<AddressViewModel> newAddress, List<PhoneViewModel> newPhones, string currentUserEmail);
	}
}