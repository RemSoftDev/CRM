using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMData.Entities
{
    public class LeadConvertedLog
    {
        public int Id { get; set; }

        public int LeadId { get; set; }

        public int CustomerId { get; set; }

        public DateTime ConvertDateTime { get; set; }

        [Column("ConvertedBy_UserId")]
        public int ConvertedByUserId { get; set; }

    }
}
