using CRM.Enums;

namespace CRM.Models
{
    public sealed class PhoneViewModel
    {
        public int? Id { get; set; }
        public string PhoneNumber { get; set; }
        public PhoneType? Type { get; set; }
    }
}