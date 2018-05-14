using System.Collections.Generic;

namespace CRMData.Entities
{
    public sealed class Lead
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }       
        public string Message { get; set; }      
        public string Discipline { get; set; }
        public string AgeGroup { get; set; }
        public string Status { get; set; }
        public string StatusNotes { get; set; }
        public string IssueRaised { get; set; }
        public int? LeadOwner { get; set; }
        public bool IsConverted { get; set; }

        public User User { get; set; }

        public IList<Phone> Phones { get; set; }
        public IList<Email> Emails { get; set; }
        public IList<Note> Notes { get; set; }

        public Lead()
        {
            this.Phones = new List<Phone>();
            this.Emails = new List<Email>();
            this.Notes = new List<Note>();
        }
    }
}