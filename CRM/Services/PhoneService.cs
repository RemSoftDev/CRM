using CRM.DAL.Entities;
using CRM.DAL.Repository;
using CRM.Enums;
using CRM.Extentions;
using System;
using System.Linq;

namespace CRM.Services
{
    public sealed class PhoneService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PhoneService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork.ValidateNotDefault(nameof(unitOfWork));
        }

        public string GetRightPartRedirectUrlByPhoneNum(string phoneNumber)
        {
            User user = _unitOfWork.UsersRepository
                .FindBy(e => e.Phones
                    .Any(p => p.PhoneNumber.Equals(phoneNumber, StringComparison.InvariantCultureIgnoreCase)) && e.UserTypeId == (int)UserType.Customer);

            if (user != null)
                return BuildRightPartUrl("Customer", "Edit", user.Id);

            Lead lead = _unitOfWork.LeadsRepository
                 .FindBy(e => e.Phones
                     .Any(p => p.PhoneNumber.Equals(phoneNumber, StringComparison.InvariantCultureIgnoreCase)));

            if (lead != null)
                return BuildRightPartUrl("Lead", "Edit", lead.Id);

            return BuildRightPartUrl("Lead", "Create");
        }

        private string BuildRightPartUrl(string controllerName, string actionName, int? id = null)
        {
            return $"{controllerName}/{actionName}/{(id.HasValue ? id.ToString() : string.Empty)}";
        }
    }
}