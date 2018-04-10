using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace crmStart.Models
{
    public class User
    {
        [Key]
        [Required]
        public int UserID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "min 6, max 100 letters")]
        public string Password { get; set; }

        public string Salt { get; } = new Random().NextDouble().ToString();

        public string Email { get; set; }

        public string Discipline { get; set; }

        public Telephone Telephone { get; set; }

        public Address address { get; set; }

        public string AgeGroup { get; set; }

        public string Status { get; set; }

        public string StatusNotes { get; set; }

        public string IssueRaised { get; set; }
    }
}