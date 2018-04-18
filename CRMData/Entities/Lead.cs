﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMData.Entities
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
        public Customer Customer { get; set; }

        public ICollection<Phone> Phones { get; set; }

        public Lead()
        {
            this.Phones = new List<Phone>();
        }
    }
}