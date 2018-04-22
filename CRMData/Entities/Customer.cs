using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMData.Entities
{
    public class Customer
    {
        [Key]
        [ForeignKey("Lead")]
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IList<Address> Addresses { get; set; }
        public IList<Phone> Phones { get; set; }
        public IList<Note> Notes { get; set; }
        public Lead Lead { get; set; }

        public Customer()
        {
            this.Addresses = new List<Address>();
            this.Phones = new List<Phone>();
            this.Notes = new List<Note>();
        }
    }
}
