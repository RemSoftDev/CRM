using System.Collections.Generic;

namespace CRM.DAL.Entities
{
    public class User 
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int UserTypeId { get; set; }
        public DUserType UserType { get; set; }

        public virtual IList<Phone> Phones { get; set; }
        public virtual IList<Address> Addresses { get; set; }
        public virtual IList<Note> Notes { get; set; }
        public virtual IList<Email> Emails { get; set; }
        public virtual IList<Call> Calls { get; set; }

        public User()
        {
            Addresses = new List<Address>();
            Phones = new List<Phone>();
            Notes = new List<Note>();
            Emails = new List<Email>();
        }
    }
}