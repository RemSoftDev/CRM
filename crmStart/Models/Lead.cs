using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace crmStart.Models
{
    public class Lead
    {
        [Key]
        [Required]
        public int LeadID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public Telephone Telephone { get; set; }

        public string Message { get; set; }
    }
}