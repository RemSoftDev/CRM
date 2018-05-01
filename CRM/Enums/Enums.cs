
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CRM.Enums
{
    public enum UserRole
    {
        AdminStaff = 0,
        Manager,
        Customer,
        Practitioner
    }

    public enum UserType
    {
        Manager = 1,
        Customer
    }

    public enum AddressType
    {
        [Display(Name = "Billing Address")]
        BillingAddress = 1,

        [Display(Name = "Contact Address")]
        ContactAddress,

        [Display(Name = "Emergency Contact Address")]
        EmergencyContactAddress

    }

    public enum PhoneType
    {
        [Display(Name = "Home Phone")]
        HomePhone = 1,

        [Display(Name = "Work Phone")]
        WorkPhone,

        [Display(Name = "Mobile Phone")]
        MobilePhone,

        [Display(Name = "Emergency Contact Phone")]
        EmergencyContactPhone,

        [Display(Name = "Fax")]
        Fax
    }
}