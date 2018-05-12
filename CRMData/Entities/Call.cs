using System;

namespace CRMData.Entities
{
    public sealed class Call
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public int? LeadId { get; set; }
        public Lead Lead { get; set; }

        public DateTime CallTime { get; set; }
    }
}
