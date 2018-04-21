
namespace CRM.Enums
{
    public enum UserRole
    {
        AdminStaff = 0,
        Manager,
        Customer,
        Practitioner
    }

    public enum AddressType
    {
        BillingAddress = 1,
        ContactAddress,
        EmergencyContactAddress

    }

    public enum PhoneType
    {
        HomePhone = 1,
        WorkPhone,
        MobilePhone,
        EmergencyContactPhone,
        Fax
    }
}