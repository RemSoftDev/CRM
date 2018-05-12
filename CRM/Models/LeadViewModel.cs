using CRM.Attributes;
using CRM.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
    public class LeadViewModel : IUser
    {
        public int Id { get; set; }

        [Required]
        [Grid(ShowOnGrid = true)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Grid(ShowOnGrid = true)]
        public string Email { get; set; }

        [Required]
        [Grid(ShowOnGrid = true)]
        public List<PhoneViewModel> Phones { get; set; }

        [Grid(ShowOnGrid = true)]
        public List<NoteViewModel> Notes { get; set; }

        [Grid(ShowOnGrid = true)]
        public string Message { get; set; }
        public int? LeadOwner { get; set; }

        [Grid(ShowOnGrid = true)]
        public string Discipline { get; set; }

        [Grid(ShowOnGrid = true)]
        public string AgeGroup { get; set; }

        [Grid(ShowOnGrid = true)]
        public string Status { get; set; }
        public string StatusNotes { get; set; }
        public string IssueRaised { get; set; }

        public LeadViewModel()
        {
            this.Notes = new List<NoteViewModel>();
        }
    }
}