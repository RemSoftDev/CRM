using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class LeadViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public List<PhoneViewModel> Phones { get; set; }
        public List<string> Notes { get; set; }
        public string Message { get; set; }
        public int? LeadOwner { get; set; }

        public LeadViewModel()
        {
            this.Notes = new List<string>();
        }
    }
}