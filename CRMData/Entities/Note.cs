using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMData.Entities
{
    public class Note
    {
        public int Id { get; set; }       
        public string Text { get; set; }
        public int? LeadId { get; set; }
        public Lead Lead { get; set; }

        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}