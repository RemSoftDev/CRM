
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
        BillingAddress = 0,
        ContactAddress,
        EmergencyContactAddress

    }

    public enum PhoneType
    {
        HomePhone = 0,
        WorkPhone,
        MobilePhone,
        EmergencyContactPhone,
        Fax
    }
}