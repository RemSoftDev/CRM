using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMData.Entities
{
    public class Email
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Subject { get; set; }
        public DateTime SentDate { get; set; }
        public bool WasReceived { get; set; }

        public int? LeadId { get; set; }
        public Lead Lead { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }        
    }
}
