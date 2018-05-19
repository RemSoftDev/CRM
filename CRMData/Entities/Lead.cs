using System.Collections.Generic;

namespace CRM.DAL.Entities
{
	public class Lead
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

		public virtual IList<Phone> Phones { get; set; }
		public virtual IList<Email> Emails { get; set; }

		public virtual IList<Call> Calls { get; set; }

		public Lead()
		{
			this.Phones = new List<Phone>();
			this.Emails = new List<Email>();
		}
	}
}