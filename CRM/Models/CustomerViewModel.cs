using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
    public sealed class CustomerViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public List<PhoneViewModel> Phones { get; set; }
        public List<NoteViewModel> Notes { get; set; }
        public List<AddressViewModel> Addresses { get; set; }

        public CustomerViewModel()
        {
            this.Notes = new List<NoteViewModel>();
            this.Addresses = new List<AddressViewModel>();
            this.Phones = new List<PhoneViewModel>();
        }
    }
}