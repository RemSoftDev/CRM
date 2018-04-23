namespace CRMData.Entities
{
    public class Phone
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }

        public int? TypeId { get; set; }
        public DPhoneType Type { get; set; }

        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public int? LeadId { get; set; }
        public Lead Lead { get; set; }
    }
}
