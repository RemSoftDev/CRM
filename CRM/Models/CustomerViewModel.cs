﻿using System.Collections.Generic;

namespace CRM.Models
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public List<PhoneViewModel> Phones { get; set; }
        public string Email { get; set; }
        public List<NoteViewModel> Notes { get; set; }

        public CustomerViewModel()
        {
            this.Notes = new List<NoteViewModel>();
        }
    }
}