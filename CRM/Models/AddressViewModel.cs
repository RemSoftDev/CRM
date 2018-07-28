using CRM.Enums;

namespace CRM.Models
{
    public class AddressViewModel
    {
        public int? Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public AddressType? Type { get; set; }
    }
}