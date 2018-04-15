using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMData.Entities
{
    public class Note
    {
        public int NoteId { get; set; }
        public int LeadId { get; set; }
        public string NoteValue { get; set; }
    }
}