using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.DAL.Entities
{
    public class LeadConvertedLog
    {
        public int Id { get; set; }

        public int LeadId { get; set; }

        public int UserId { get; set; }

        public DateTime ConvertDateTime { get; set; }

        [Column("ConvertedBy_UserId")]
        public int ConvertedByUserId { get; set; }

    }
}
