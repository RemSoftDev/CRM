using CRM.Attributes;
using CRM.Enums;

namespace CRM.Models
{
    public sealed class PhoneViewModel
    {
        public int? Id { get; set; }

        [Grid(ShowOnGrid = true)]
        public string PhoneNumber { get; set; }
        public PhoneType? Type { get; set; }
    }
}