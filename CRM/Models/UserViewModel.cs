using CRM.Attributes;
using CRM.Enums;
using CRM.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
    public class UserViewModel : IUser
    {
        public int Id { get; set; }

        [Grid(ShowOnGrid = true)]
        public string Title { get; set; }

        [Required]
        [Grid(ShowOnGrid = true)]
        public string FirstName { get; set; }

        [Required]
        [Grid(ShowOnGrid = true)]
        public string LastName { get; set; }

        [Grid(ShowOnGrid = true)]
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public string Password { get; set; }

        [Grid(ShowOnGrid = true)]
        public List<PhoneViewModel> Phones { get; set; }
        [Grid(ShowOnGrid = true)]
        public List<NoteViewModel> Notes { get; set; }
        [Grid(ShowOnGrid = true)]
        public List<AddressViewModel> Addresses { get; set; }

        public UserViewModel()
        {
            this.Notes = new List<NoteViewModel>();
            this.Addresses = new List<AddressViewModel>();
            this.Phones = new List<PhoneViewModel>();
        }
    }
}