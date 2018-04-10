using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace crmStart.Models
{
    public class Telephone
    {
        [Key]
        [Required]
        public int TelephoneID { get; set; }

        public string Number { get; set; }

        public TelephoneType telephoneType { get; set; }
    }
}