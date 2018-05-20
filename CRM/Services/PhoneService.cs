using CRM.DAL.Entities;
using CRM.DAL.Repository;
using CRM.Extentions;
using System;

namespace CRM.Services
{
    public sealed class PhoneService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PhoneService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork.ValidateNotDefault(nameof(unitOfWork));
        }

        public string GetRightPartRedirectUrl(string phoneNumber)
        {
            Phone phone = _unitOfWork.PhonesRepository
                .FindBy(e => e.PhoneNumber.Equals(phoneNumber, StringComparison.InvariantCultureIgnoreCase));

            if (phone != null && phone.UserId.HasValue)
                return BuildRightPartUrl("Customer", "Edit", phone.UserId);
            
            if (phone != null && phone.LeadId.HasValue)
                return BuildRightPartUrl("Lead", "Edit", phone.LeadId);

            return BuildRightPartUrl("Lead", "Create");
        }

        private string BuildRightPartUrl(string controllerName, string actionName, int? id = null)
        {
            return $"{controllerName}/{actionName}/{(id.HasValue ? id.ToString() : string.Empty)}";
        }
    }
}