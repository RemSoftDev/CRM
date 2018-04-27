using CRM.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }
        public UserRole Role { get; set; }
        public string Password { get; set; }

        public List<PhoneViewModel> Phones { get; set; }
        public List<NoteViewModel> Notes { get; set; }
        public List<AddressViewModel> Addresses { get; set; }

        public UserViewModel()
        {
            this.Notes = new List<NoteViewModel>();
            this.Addresses = new List<AddressViewModel>();
            this.Phones = new List<PhoneViewModel>();
        }
    }
}