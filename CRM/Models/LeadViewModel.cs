using CRM.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
    public class LeadViewModel : IUser
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public List<PhoneViewModel> Phones { get; set; }
        public List<NoteViewModel> Notes { get; set; }
        public string Message { get; set; }
        public int? LeadOwner { get; set; }

        public string Discipline { get; set; }
        public string AgeGroup { get; set; }
        public string Status { get; set; }
        public string StatusNotes { get; set; }
        public string IssueRaised { get; set; }

        public LeadViewModel()
        {
            this.Notes = new List<NoteViewModel>();
        }
    }
}