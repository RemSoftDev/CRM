using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace crmStart.Models
{
    public class Address
    {
        [Key]
        [Required]
        public int AddressID { get; set; }

        public string Line1 { get; set; }
        public string Line2 { get; set; }

        public string Town { get; set; }

        public string Country { get; set; }

        public string PostCode { get; set; }

        public AddressType addressType { get; set; }
    }
}